using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using Mono.Cecil;

if (args.Length != 4 || args[0] is not ("original" or "patched" or "rentals"))
    throw new ArgumentException("Usage: Checks <original|patched|rentals> <DLL> <GameManaged> <Profile>");
var legacy = args[0] == "original";
var dll = Path.GetFullPath(args[1]);
var managed = Path.GetFullPath(args[2]);
var profile = Path.GetFullPath(args[3]);
var paths = new[]
{
    Path.GetDirectoryName(dll)!, managed, Path.Combine(profile, "BepInEx/core"),
    Path.Combine(profile, "BepInEx/plugins/MMHOOK"),
    Path.Combine(profile, "BepInEx/plugins/RiskofThunder-R2API_Core/R2API.Core"),
    Path.Combine(profile, "BepInEx/plugins/RiskofThunder-R2API_CommandHelper/R2API.CommandHelper")
};
AppDomain.CurrentDomain.AssemblyResolve += (_, eventArgs) =>
{
    var name = new AssemblyName(eventArgs.Name).Name + ".dll";
    foreach (var directory in paths)
    {
        var candidate = Path.Combine(directory, name);
        if (File.Exists(candidate)) return Assembly.LoadFrom(candidate);
    }
    return null;
};

// Use separate processes for the original and patched assembly. A failed static
// initializer is permanently failed for that type in its current load context.
var assembly = Assembly.LoadFrom(dll);
var chat = assembly.GetType("ShareSuite.ChatHandler", true)!;
var reproduced = false;
try
{
    RuntimeHelpers.RunClassConstructor(chat.TypeHandle);
}
catch (TypeInitializationException error) when (legacy && error.InnerException is AmbiguousMatchException)
{
    reproduced = true;
    Console.WriteLine("PASS: original DLL reproduces AmbiguousMatchException in ChatHandler.");
}
if (legacy)
{
    if (!reproduced) throw new Exception("Original DLL did not reproduce the reported failure.");
    return;
}

var callback = (Delegate)chat.GetField("SendPickupMessage")!.GetValue(null)!;
var parameters = callback.Method.GetParameters();
if (callback.Method.Name != "SendPickupMessage" || parameters.Length != 2 ||
    parameters[0].ParameterType.FullName != "RoR2.CharacterMaster" ||
    parameters[1].ParameterType.FullName != "RoR2.UniquePickup")
    throw new Exception("Incorrect pickup-message overload selected.");
Console.WriteLine("PASS: patched static initializer completed; exact binding = " + callback.Method);

var resolver = new DefaultAssemblyResolver();
foreach (var directory in paths) resolver.AddSearchDirectory(directory);
using var module = ModuleDefinition.ReadModule(dll, new ReaderParameters { AssemblyResolver = resolver });
var checkedMembers = 0;
var unresolved = new List<string>();
IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> types)
{
    foreach (var type in types)
    {
        yield return type;
        foreach (var nested in AllTypes(type.NestedTypes)) yield return nested;
    }
}
var references = module.GetMemberReferences().Concat(AllTypes(module.Types)
    .SelectMany(t => t.Methods).Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MemberReference>())
    .Where(m => m is MethodReference or FieldReference)
    .DistinctBy(m => m.FullName);
foreach (var member in references)
{
    var scope = member.DeclaringType.Scope.Name;
    if (!(scope == "RoR2" || scope == "MMHOOK_RoR2" || scope == "HGCSharpUtils" ||
          scope == "HGUnityUtils" || scope == "com.unity.multiplayer-hlapi.Runtime" ||
          scope.StartsWith("UnityEngine") || scope.StartsWith("R2API") || scope == "BepInEx" ||
          scope == "Mono.Cecil" || scope == "MonoMod.Utils")) continue;
    checkedMembers++;
    try
    {
        var resolved = member switch
        {
            MethodReference method => (MemberReference?)method.Resolve(),
            FieldReference field => field.Resolve(),
            _ => throw new Exception("Unsupported member: " + member.FullName)
        };
        if (resolved is null) unresolved.Add(member.FullName);
    }
    catch (Exception error) { unresolved.Add(member.FullName + " -> " + error.Message); }
}
if (unresolved.Count != 0)
    throw new Exception("Unresolved real dependency members:\n" + string.Join("\n", unresolved));
Console.WriteLine($"PASS: all {checkedMembers} game, hook and plugin dependency members resolve.");

// Validate actual emitted hook registrations, including cleanup, rather than
// relying on the build or matching source spelling alone.
var chatDefinition = module.Types.Single(t => t.FullName == "ShareSuite.ChatHandler");
var hookCalls = chatDefinition.Methods.Single(m => m.Name == "Hook").Body.Instructions
    .Select(i => i.Operand).OfType<MethodReference>().Where(m => m.Name.StartsWith("add_")).ToArray();
var unhookCalls = chatDefinition.Methods.Single(m => m.Name == "UnHook").Body.Instructions
    .Select(i => i.Operand).OfType<MethodReference>().Where(m => m.Name.StartsWith("remove_")).ToArray();
if (hookCalls.Length != 2 || unhookCalls.Length != 2 ||
    !hookCalls.Any(m => m.Name == "add_SendPickupMessage_CharacterMaster_UniquePickup") ||
    !hookCalls.Select(m => m.DeclaringType.FullName + "::" + m.Name[4..]).Order()
        .SequenceEqual(unhookCalls.Select(m => m.DeclaringType.FullName + "::" + m.Name[7..]).Order()))
    throw new Exception("Pickup hook/unhook symmetry or overload-specific registration failed.");
Console.WriteLine("PASS: exact pickup hook and unhook targets are valid and symmetric.");
if (args[0] != "rentals")
{
    Console.WriteLine("Scope: dependency, MethodSpec and static-initializer checks; Unity gameplay not executed.");
    return;
}

// Exercise the actual rental wrappers with real game and MMHOOK types. The
// Unity controller is deliberately uninitialized: a valid early pass-through
// must not read a body, native components, catalogs or sharing configuration.
var game = callback.Method.DeclaringType!.Assembly;
var controllerType = game.GetType("RoR2.GenericPickupController", true)!;
var pickupType = game.GetType("RoR2.UniquePickup", true)!;
foreach (var wrapperName in new[] { "ShareSuite.ItemSharingHooks", "ShareSuite.EquipmentSharingHooks" })
{
    var method = assembly.GetType(wrapperName, true)!.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .Single(m => m.Name is "OnGrantItem" or "OnGrantEquipment");
    var controller = RuntimeHelpers.GetUninitializedObject(controllerType);
    var pickup = Activator.CreateInstance(pickupType)!;
    pickupType.GetField("isRental")!.SetValue(pickup, true);
    // The public setter initializes network/display state, which needs Unity.
    // Seed its actual backing field; the wrapper reads the real public getter.
    controllerType.GetField("_pickupState", BindingFlags.Instance | BindingFlags.NonPublic)!
        .SetValue(controller, pickup);
    var calls = 0;
    Action<object?, object?> next = (self, body) =>
    {
        if (!ReferenceEquals(self, controller) || body is not null)
            throw new Exception("Rental pass-through changed its arguments.");
        calls++;
    };
    var delegateType = method.GetParameters()[0].ParameterType;
    var delegateParameters = delegateType.GetMethod("Invoke")!.GetParameters()
        .Select(p => Expression.Parameter(p.ParameterType)).ToArray();
    var forward = Expression.Lambda(delegateType,
        Expression.Invoke(Expression.Constant(next), delegateParameters.Select(p => Expression.Convert(p, typeof(object)))),
        delegateParameters).Compile();
    method.Invoke(null, new object?[] { forward, controller, null });
    if (calls != 1) throw new Exception($"Rental handler {wrapperName} forwarded {calls} times.");
    Console.WriteLine($"PASS: {wrapperName} forwards a rental exactly once before catalog/body/config access.");
}
Console.WriteLine("Scope: real dependency and initialization checks; Unity gameplay not executed.");
