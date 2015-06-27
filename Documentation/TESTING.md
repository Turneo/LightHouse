# Testing (LightHouse)

The tests in LightHouse are written when possible in a portable class library and then for each platform a runner is created which will execute them in the correct environment.

Here is an example for how the LightHouse Core components are structured:

### Path: Testing/Portable/Profile78

1. **Project Name:** LightHouse.Core.Testing.Profile78.csproj
2. **Description:** Contains the portable tests.

### Path: Testing/Native/NET45

1. **Project Name:** LightHouse.Core.Testing.Runner.NET45.csproj
2. **Description:** Contains the runners for executing the portable tests in .NET Framework 4.5.

### Path: Testing/Native/iOS

1. **Project Name:** LightHouse.Core.Testing.Runner.iOS.csproj
2. **Description:** Contains the runners for executing the portable tests in iOS.

The tests in the runner need to be marked with the *TestCase* attribute. Additionally Scenarios can be created to execute the test in different combinations of extensions. Finally the test can be marked as CI (continuous 
integration) or Nightly (nightly build). CI and Nightly determin when it will be tested or built.

Here is an example:

```
        [TestCase]
        [CI]
        [Scenario("Standard")]
        public new void MergePersonAsDataObject()
        {
            TestConfiguration.Merger.MergePersonAsDataObject();
        }
```

The TestConfiguration keeps a singleton reference to the Merger class, which in the portable class library contains all the test that can be executed.

### Creating a Test Project for Unit Testing

In Visual Studio create a portable class library (Profile78) project. Then the necessary libraries and references need to be added. The class should have the same name as the class that is to be tested. The method should also be named the same as the method that has to be tested. To run the tests a test runner is needed.

### Creating a Test Runner Project

Create a class library project in Visual Studio. Add the correct references, the test logic assembly and the assemblies that are going to be tested. After that, add the xUnit libraries via the nuget package manager. The "Fact" attribute marks that this method is a test method, which is a standard xUnit feature.

An example of a test runner class is shown below:

```
namespace LightHouse.Core.Testing.Runner
{
	public class Locale
	{
		private LightHouse.Core.Testing.Localization.Locale localeTester;
		
		public Locale()
		{
			localeTester = new LightHouse.Core.Testing.Localization.Locale();
		}
		
		[Fact]
		public void SetCulture()
		{
			localeTester.SetCulture();
		}
	}
}
```

### Setting the Test Runner Environment

In LightHouse it is possible to run the same test logic in different environment settings.

For Example, I would like to run my Unit Test with different storage and execution operators. To do that the corresponding operators need to be added to an environment. It is possible to set the environment in the constructor of the test runner or create a static class called "TestConfiguration" and implement the static method "SetEnvironment", which can be called in the test runner constructor.

```
namespace LightHouse.Core.Testing.Runner
{
	public static class TestConfiguration
	{
		public static void SetEnvironment()
		{
			LightHouse.Base.Testing.Operator.ClearEnvironments();
			
			var embeddedStorageOperator = new LightHouse.Storage.Embedded.Operator());
			var embeddedExecutionOperator = new LightHouse.Execution.Embedded.Operator());
			var serverStorageOperator = new LightHouse.Storage.Server.Operator());
			var serverExecutionOperator = new LightHouse.Execution.Server.Operator());
			
			LightHouse.Base.Testing.Operator.AddEnvironment("Embedded", new TestEnvironment()
			{
				StorageOperator = embeddedStorageOperator,
				ExecutionOperator = embeddedExecutionOperator
			});
			LightHouse.Base.Testing.Operator.AddEnvironment("Server", new TestEnvironment()
			{
				StorageOperator = serverStorageOperator,
				ExecutionOperator = serverExecutionOperator
			});
		}
	}	
}
```

It is now possible to call the SetEnvironment method in the test constructor and modify the sample test runner above as seen below:

```
namespace LightHouse.Core.Testing.Runner
{
	public class TestRunner
	{
		public TestRunner()
		{
			TestConfiguration.SetEnvironment();
		}
		
		[TestCase]
		[Scenario("Embedded")]
		[Scenario("Server")]
		public void ExecutingMoveNext()
		{
			TestConfiguration.ContractEnumerator.ExecutingMoveNext();
		}
	}
}
```

This setting will run the same test for both the embedded and the server environments.

### Adding Test Parameters

Using the Scenario attribute, it is not only possible to configure the runner environment, it is also possible to add test parameters there.

Here is an example:

```
        [TestCase]
        [Scenario("Embedded", new object[] { "Perter", 1 })]
        [Scenario("Embedded", new object[] { "Walter", 5 })]
        public void TestParameters(String name, int amount)
        {
            
        }
```

### Mocking

Sometimes mocking objects, services or operators will be created in a test. They can be easily created in the test logic project. Then, the mocking operators can be set in the environment settings in ConfigurationFixture.

### Naming the Unit Test

The name of the Unit Test is also important. In the example above the name of the Unit Test is MergePersonAsDataObject(). The name of the Unit Test should describe what the Unit Test should test or check. In this example 
it is clear, that the Unit Test will merge a person as a data object.

LightHouse is a registered trademark of Turneo AG (www.turneo.com).
