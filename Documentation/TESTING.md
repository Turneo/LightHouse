# Testing (LightHouse)

The tests in LightHouse are written when possible in a portable class library and then for each platform a runner is created which will execute them in the correct environment.

As an example for the LightHouse Core components the tests are structured the following way:

### Path: Testing/Portable/Profile78

1. **Project Name:** LightHouse.Core.Testing.Profile78.csproj
2. **Description:** Contains the portable tests.

### Path: Testing/Native/NET45

1. **Project Name:** LightHouse.Core.Testing.Runner.NET45.csproj
2. **Description:** Contains the runners for executing the portable tests in .NET Framework 4.5.

### Path: Testing/Native/iOS

1. **Project Name:** LightHouse.Core.Testing.Runner.iOS.csproj
2. **Description:** Contains the runners for executing the portable tests in iOS.

The tests in the runner need to be marked with the *TestCase* attribute. Additionaly Scenarios can be created to execute the test in different combinations of extensions. Finally the test can be marked as CI (continous integration) or Nightly (nightly build).

Here an example:

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

LightHouse is a registered trademark of Turneo AG (www.turneo.com).
