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

### Creating a Test Project to test the Logic

In Visual Studio create a portable class library (Profile78) project. Then the necessary libraries and references need to be added.
The class should have the same name as the class that is to be tested. The method should also be named the same as the method that has to be tested.
To run the tests a test runner is needed.

### Creating a Test Runner Project

Create a class library project in Visual Studio. Add the correct references, the test logic assembly and the assemblies that are going to be tested. After that, add the xUnit libraries via the nuget package manager.
The "Fact" attribute marks that this method is a test method, which is a standard xUnit feature.

An example of a test runner class is shown below:

namespace LightHouse.Core.Testing.Runner
{
	public class Locale
	{
		private LightHouse.Core.Testing.localization.Locale testing;
		
		public Locale()
		{
			this.testing = new LightHouse.Core.Testing.localization.Locale();
		}
		
		[Fact]
		public void SetCulture()
		{
			this.testing.SetCulture();
		}
	}
}

### Setting the Test Runner Environment

In LightHouse it is possible to run the same test logic in different environment settings.

For Example, I would like to run my Unit Test in builders.
To do that, LightHouse.Core.Building.Native and LightHouse.Core.Building.Portable have to be added to the references of the runner.
It is possible to set the environment in the constructor of the test runner or create a static class called "TestConfiguration" and implement the static method "SetEnvironment", which can be called in the test runner constructor.

namespace LightHouse.Core.Testing.Runner
{
	public static class TestConfiguration
	{
		public static void SetEnvironment()
		{
			LightHouse.Base.Testing.Operator.ClearEnvironments();
			
			var nativeBuilder = new LightHouse.Core.Building.Native.Builder(new LightHouse.Core.DataObject());
			var portableBuilder = new LightHouse.Core.Building.Portable.Builder(new LightHouse.Core.DataObject());
			
			LightHouse.Testing.Elite.Core.Builder = portableBuilder;
			
			LightHouse.Base.Testing.Operator.AddEnvironment("NativeBuilder", new TestEnvironment()
			{
				Builder = nativeBuilder
			});
			LightHouse.Base.Testing.Operator.AddEnvironment("PortableBuilder", new TestEnvironment()
			{
				Builder = portableBuilder
			});
		}
	}	
}

It is now possible to call the SetEnvironment method in the test constructor and modify the sample test runner above as seen below:

namespace LightHouse.Core.Testing.Runner
{
	public class TestRunner
	{
		public TestRunner()
		{
			TestConfiguration.SetEnvironment();
		}
		
		[TestCase]
		[Scenario("NativeBuilder")]
		[Scenario("PortableBuilder")]
		public void ExecutingMoveNext()
		{
			TestConfiguration.ContractEnumerator.ExecutingMoveNext();
		}
	}
}

This setting will run the same test for both the NativeBuilder and the PortableBuilder environments.

### Adding Test Parameters

Using the Scenario attribute, it is not only possible to configure the runner environment, it is also possible to add test parameters there.

Here is an example:

```
        [TestCase]
        [Scenario("PortableBuilder", new object[] { "Perter", 1 })]
        [Scenario("PortableBuilder", new object[] { "Walter", 5 })]
        public void TestParameters(String name, int amount)
        {
            
        }
```

### Mocking

Sometimes mocking objects, services or operators will be created in a test. They can be easily created in the test logic project. Then, the mocking operators can be set in the environment settings in ConfigurationFixture.

### Naming the Unit Test

The name of the Unit Test is also important. In the example above the name of the Unit Test is MergePersonAsDataObject(). The name of the Unit Test should describe what the Unit Test should test or check. In this example 
it is clear, that the Unit Test will merge a person as a data object.

### Running the Unit Test

To run the Unit Test or Tests, the Test Explorer is used in Visual Studio. To open the Test Explorer, the "Test" tab at the top of the screen needs to be opened. Then go to "Windows" and under "Windows" the Test Explorer can 
be found.

Once the Test Explorer is open, the Unit Tests can be run. If there are multiple Unit Tests, there is an option to run them individually or all at the same time.
The result of the test is then displayed in the Test Explorer. The Unit Test has either passed or failed. This represented with a green tick (passed) or red cross (failed). Other statistics are also displayed, like the amount
of time taken for the test to run. 

### Unit Test Code Coverage

Code Coverage is a feature in Visual Studio but only for Visual Studio Ultimate or Visual Studio Premium users. If the user has a different version of Visual Studio, the Code Coverage feature will not be available. To find out 
which version of Visual Studio is currently being used, click on the "Help" tab at the top of the screen. Then click on "About Visual Studio". This will open a window with information about Visual Studio. At the top of the 
screen, under the logo, the full name of the product will be displayed. For example, "Microsoft Visual Studio Professional 2013".

Although the Unit Test may be successful, it might not be fully optimised. The Code Coverage shows how much of the code is tested by the Unit Test.
The Code Coverage element displays exactly which parts of the code are tested by the Unit Test in percent. It also highlights the code which is not covered.

To find the Code Coverage feature in Visual Studio Ultimate or Premium, go to the "Test" tab at the top of the screen. There will then be an option called "Analyze Code Coverage". This will lead to a choice of two options.
These option are named "Selected Tests" and "All Tests". Click on either one to analyze the Code Coverage of the selected tests or all of them.

LightHouse is a registered trademark of Turneo AG (www.turneo.com).