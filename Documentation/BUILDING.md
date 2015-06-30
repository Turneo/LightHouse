# Building (LightHouse)

##Overview

The build definitions are currently run by the internal infrastructure of Turneo AG and are therefore not accessible to the public.

Each solution has its own build definitions for 32 and 64 Bit Environments. Normally a build definition for continuos integration and one for nightly is created.

##Configuration of Build Definition

The following configurations are required for each build definition:

###General
The name of the build definition must follow the following pattern, "Trigger - Solution Name (Platform target)". Following this pattern, LightHouse.Core should therefore look like this: 

"CI - LightHouse Core (x86)". 

In the example above there is used the Continuous Integration trigger. If the nightly trigger 
would be used, the "CI" would be replaced by "Nightly". This is followed up by the name of
the solution and the Platform target it will be built for (x86 or x64).

###Trigger
Depending on the build definition type, the trigger needs to be set to continous integration or schedule.
The scheduled trigger time for the nightly builds is currently set at 1am Western Europe Summer Time (UTC +02:00).

###Source Settings
In source settings you can select the default LightHouse git repository. 

###Build Defaults
Select the build controller you want to use. 

###Process
Set the following values:

Clean Repository: False
Solution to build: Path of the solution you want to build.
Configuration to build: Configuration should always be "Release".
Platform to build: The platform target that was defined in the build definitions name.
MSBuild arguments: /p:GenerateProjectSpecificOptputFolder=False
