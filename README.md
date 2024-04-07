# DelegateForGenericMethodExample

Demo for https://github.com/dotnet/runtime/issues/100748

```
Created Instance of ClassDelegate for GenericFoo
Type Arguments: System.String, System.Int32
Target Type: C1
Open Generic Method: Tout GenericFoo[Tin,Tout](Tin) declared on C1
Closed Generic Method: Int32 GenericFoo[String,Int32](System.String) declared on C1
Creating....
Created Delegate: ClassDelegate


Created Instance of ClassDelegate for NonGenericFoo
Target Type: C1
Method: Int32 NonGenericFoo(System.String) declared on C1
Creating....
Created Delegate: ClassDelegate


Created Instance of InterfaceDelegate for NonGenericFoo
Target Type: I1
Method: Int32 NonGenericFoo(System.String) declared on I1
Creating....
Created Delegate: InterfaceDelegate


Created Instance of InterfaceImplementationDelegate for NonGenericFoo
Target Type: ImplementationI1
Method: Int32 NonGenericFoo(System.String) declared on ImplementationI1
Creating....
Created Delegate: InterfaceImplementationDelegate


Created Instance of InterfaceHiddenDelegate for GenericFoo
Type Arguments: System.String, System.Int32
Target Type: HiddenI1
Open Generic Method: Tout GenericFoo[Tin,Tout](Tin) declared on HiddenI1
Closed Generic Method: Int32 GenericFoo[String,Int32](System.String) declared on HiddenI1
Creating....
Created Delegate: InterfaceHiddenDelegate


Created Instance of InterfaceHiddenDelegate for NonGenericFoo
Target Type: HiddenI1
Method: Int32 NonGenericFoo(System.String) declared on HiddenI1
Creating....
Created Delegate: InterfaceHiddenDelegate


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
```