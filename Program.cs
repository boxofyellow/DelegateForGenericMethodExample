// Demo for https://github.com/dotnet/runtime/issues/100748

public interface I1
{
    // Any attempt to create a delegate for these generic method (or any method that implements this) will fail
    // All the others are fine
    public Tout GenericFoo<Tin, Tout>(Tin input);

    // It does not matter where/if the generic arguments are used in rest of the declaration
    public void GenericBar<X>() { }
    public X GenericBaz<X>() => default!;
    public void GenericQux<X>(X input) { }

    // We will see no problem creating delegates for non generic methods
    public int NonGenericFoo(string input);
}

public class C1
{
    public Tout GenericFoo<Tin, Tout>(Tin input) => default!;
    public int NonGenericFoo(string input) => default;
}

public class ImplementationI1 : I1
{
    public Tout GenericFoo<Tin, Tout>(Tin input) => default!;
    public int NonGenericFoo(string input) => default;
}

public class HiddenI1 : I1
{
    public Tout GenericFoo<Tin, Tout>(Tin input) => default!;
    public int NonGenericFoo(string input) => default;

    Tout I1.GenericFoo<Tin, Tout>(Tin input) => default!;
    int I1.NonGenericFoo(string input) => default;
}

/*
    Delegates for NonGenericFoo (and GenericFoo<string, int>)
*/

public delegate int InterfaceDelegate(I1 i1, string input);
public delegate int ClassDelegate(C1 c1, string input);
public delegate int InterfaceImplementationDelegate(ImplementationI1 i1, string input);
public delegate int InterfaceHiddenDelegate(HiddenI1 i1, string input);

/*
    Delegates for Bar, Baz and Qux
*/

public delegate void BarDelegate<X>(I1 it);
public delegate X BazDelegate<X>(I1 it);
public delegate void QuxDelegate<X>(I1 it, X input);


public class Program
{
    public static D CreateDelegateForClosedGenericMethod<D>(string name, params Type[] typeArguments)
        where D : Delegate
    {
        Console.WriteLine();
        var delegateType = typeof(D);
        Console.WriteLine($"Created Instance of {delegateType} for {name}");
        Console.WriteLine($"Type Arguments: {string.Join(", ", (IEnumerable<Type>)typeArguments)}");

        // Find the invoke method so that get both target type
        var invokeMethod = delegateType.GetMethod("Invoke")!;

        var parameters = invokeMethod.GetParameters();
        var targetType = parameters.First().ParameterType;
        Console.WriteLine($"Target Type: {targetType}");

        var genericMethod = targetType.GetMethod(name)!;

        Console.WriteLine($"Open Generic Method: {genericMethod} declared on {genericMethod.DeclaringType}");

        // Now Close the method we found using the so that we can create a delegate for that
        var method = genericMethod.MakeGenericMethod(typeArguments);
        Console.WriteLine($"Closed Generic Method: {method} declared on {method.DeclaringType}");

        Console.WriteLine("Creating....");

        //
        // This is where the NotSupportedException is thrown, but only if the method part of an interface
        //
        D result = method.CreateDelegate<D>();
        Console.WriteLine($"Created Delegate: {result}");

        Console.WriteLine();
        return result;
    }

    public static D CreateDelegateNonGenericMethod<D>(string name)
        where D : Delegate
    {
        Console.WriteLine();
        var delegateType = typeof(D);
        Console.WriteLine($"Created Instance of {delegateType} for {name}");

        // Find the invoke method so that get both target type
        var invokeMethod = delegateType.GetMethod("Invoke")!;

        var parameters = invokeMethod.GetParameters();
        var targetType = parameters.First().ParameterType;
        Console.WriteLine($"Target Type: {targetType}");

        var method = targetType.GetMethod(name)!;
        Console.WriteLine($"Method: {method} declared on {method.DeclaringType}");

        Console.WriteLine("Creating....");

        //
        // This does NOT throw, even if method was defined on an interface
        //
        D result = method.CreateDelegate<D>();
        Console.WriteLine($"Created Delegate: {result}");

        Console.WriteLine();
        return result;
    }

    public static void Main()
    {
        var genericName = nameof(I1.GenericFoo);
        var nonGenericName = nameof(I1.NonGenericFoo);

        // These are all fine
        CreateDelegateForClosedGenericMethod<ClassDelegate>(genericName, typeof(string), typeof(int));
        CreateDelegateNonGenericMethod<ClassDelegate>(nonGenericName);
        CreateDelegateNonGenericMethod<InterfaceDelegate>(nonGenericName);
        CreateDelegateNonGenericMethod<InterfaceImplementationDelegate>(nonGenericName);
        CreateDelegateForClosedGenericMethod<InterfaceHiddenDelegate>(genericName, typeof(string), typeof(int));
        CreateDelegateNonGenericMethod<InterfaceHiddenDelegate>(nonGenericName);

        try
        {
            /* This one throws
            Created Instance of InterfaceDelegate for GenericFoo
            Type Arguments: System.String, System.Int32
            Target Type: I1
            Open Generic Method: Tout GenericFoo[Tin,Tout](Tin) declared on I1
            Closed Generic Method: Int32 GenericFoo[String,Int32](System.String) declared on I1
            Creating....
            System.NotSupportedException: Specified method is not supported.
                at System.Delegate.BindToMethodInfo(Object target, IRuntimeMethodInfo method, RuntimeType methodType, DelegateBindingFlags flags)
                at System.Delegate.CreateDelegateInternal(RuntimeType rtType, RuntimeMethodInfo rtMethod, Object firstArgument, DelegateBindingFlags flags)
                at System.Reflection.RuntimeMethodInfo.CreateDelegateInternal(Type delegateType, Object firstArgument, DelegateBindingFlags bindingFlags)
                at System.Reflection.MethodInfo.CreateDelegate[T]()
                at Program.CreateDelegateForClosedGenericMethod[D](String name, Type[] typeArguments)
                at Program.Main()
            */    
            CreateDelegateForClosedGenericMethod<InterfaceDelegate>(genericName, typeof(string), typeof(int));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        try
        {
            /*
            This will throw
            Created Instance of InterfaceImplementationDelegate for GenericFoo
            Type Arguments: System.String, System.Int32
            Target Type: ImplementationI1
            Open Generic Method: Tout GenericFoo[Tin,Tout](Tin) declared on ImplementationI1
            Closed Generic Method: Int32 GenericFoo[String,Int32](System.String) declared on ImplementationI1
            Creating....
            System.NotSupportedException: Specified method is not supported.
                at System.Delegate.BindToMethodInfo(Object target, IRuntimeMethodInfo method, RuntimeType methodType, DelegateBindingFlags flags)
                at System.Delegate.CreateDelegateInternal(RuntimeType rtType, RuntimeMethodInfo rtMethod, Object firstArgument, DelegateBindingFlags flags)
                at System.Reflection.RuntimeMethodInfo.CreateDelegateInternal(Type delegateType, Object firstArgument, DelegateBindingFlags bindingFlags)
                at System.Reflection.MethodInfo.CreateDelegate[T]()
                at Program.CreateDelegateForClosedGenericMethod[D](String name, Type[] typeArguments)
                at Program.Main()
            */
            CreateDelegateForClosedGenericMethod<InterfaceImplementationDelegate>(genericName, typeof(string), typeof(int));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        try
        {
            /*
            This will throw
            Created Instance of BarDelegate`1[System.Int32] for GenericBar
            Type Arguments: System.Int32
            Target Type: I1
            Open Generic Method: Void GenericBar[X]() declared on I1
            Closed Generic Method: Void GenericBar[Int32]() declared on I1
            Creating....
            System.NotSupportedException: Specified method is not supported.
                at System.Delegate.BindToMethodInfo(Object target, IRuntimeMethodInfo method, RuntimeType methodType, DelegateBindingFlags flags)
                at System.Delegate.CreateDelegateInternal(RuntimeType rtType, RuntimeMethodInfo rtMethod, Object firstArgument, DelegateBindingFlags flags)
                at System.Reflection.RuntimeMethodInfo.CreateDelegateInternal(Type delegateType, Object firstArgument, DelegateBindingFlags bindingFlags)
                at System.Reflection.MethodInfo.CreateDelegate[T]()
                at Program.CreateDelegateForClosedGenericMethod[D](String name, Type[] typeArguments)
                at Program.Main()
            */
            CreateDelegateForClosedGenericMethod<BarDelegate<int>>(nameof(I1.GenericBar), typeof(int));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        
        try
        {
            /*
            Created Instance of BazDelegate`1[System.Int32] for GenericBaz
            Type Arguments: System.Int32
            Target Type: I1
            Open Generic Method: X GenericBaz[X]() declared on I1
            Closed Generic Method: Int32 GenericBaz[Int32]() declared on I1
            Creating....
            System.NotSupportedException: Specified method is not supported.
                at System.Delegate.BindToMethodInfo(Object target, IRuntimeMethodInfo method, RuntimeType methodType, DelegateBindingFlags flags)
                at System.Delegate.CreateDelegateInternal(RuntimeType rtType, RuntimeMethodInfo rtMethod, Object firstArgument, DelegateBindingFlags flags)
                at System.Reflection.RuntimeMethodInfo.CreateDelegateInternal(Type delegateType, Object firstArgument, DelegateBindingFlags bindingFlags)
                at System.Reflection.MethodInfo.CreateDelegate[T]()
                at Program.CreateDelegateForClosedGenericMethod[D](String name, Type[] typeArguments)
                at Program.Main()
            */
            CreateDelegateForClosedGenericMethod<BazDelegate<int>>(nameof(I1.GenericBaz), typeof(int));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        try
        {
            /*
            This will throw
            Created Instance of QuxDelegate`1[System.Int32] for GenericQux
            Type Arguments: System.Int32
            Target Type: I1
            Open Generic Method: Void GenericQux[X](X) declared on I1
            Closed Generic Method: Void GenericQux[Int32](Int32) declared on I1
            Creating....
            System.NotSupportedException: Specified method is not supported.
                at System.Delegate.BindToMethodInfo(Object target, IRuntimeMethodInfo method, RuntimeType methodType, DelegateBindingFlags flags)
                at System.Delegate.CreateDelegateInternal(RuntimeType rtType, RuntimeMethodInfo rtMethod, Object firstArgument, DelegateBindingFlags flags)
                at System.Reflection.RuntimeMethodInfo.CreateDelegateInternal(Type delegateType, Object firstArgument, DelegateBindingFlags bindingFlags)
                at System.Reflection.MethodInfo.CreateDelegate[T]()
                at Program.CreateDelegateForClosedGenericMethod[D](String name, Type[] typeArguments)
                at Program.Main()
            */
            CreateDelegateForClosedGenericMethod<QuxDelegate<int>>(nameof(I1.GenericQux), typeof(int));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}