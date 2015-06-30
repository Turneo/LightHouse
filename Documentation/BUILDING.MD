# Build Definition (LightHouse)

##Overview

The build definitions are currently run by the internal infrastructure of Turneo AG and are therefore not accessible to the public.

Build Definitions are used to build and test the projects in a solution. It is possible to 
select whether it should be built at a specific time or right after a commit. Each solution has 
its own Build Definitions for 32 and 64 Bit.


##Create Build Definition
It takes the following steps to create a new build definition using visual studio:
Go to Team Explorer -> Build -> New Build Definition

The following points should be configured before saving the build Definition:

###General
The name of the Build Definition must follow the defined pattern, "Trigger - Solution Name (Platform target)". 
Following this pattern, LightHouse.Core should therefore look like this: "CI - LightHouse Core (x86)". 
First, the type of trigger used in the Build Definition. 
In the example above there is used the Continuous Integration trigger. If the nightly trigger 
would be used, the "CI" would be replaced by "Nightly". This is followed up by the name of
the solution and the Platform target it will be built for (x86 or x64).

###Trigger
Each Solution has to get a Build Definition for Nightly and Continuous Integration.
The build time for the nightly ones is 1am Western Europe Summer Time (UTC +02:00).

###Source Settings
In Source Settings you can set which repository should be built. 

###Build Defaults
Select the Build controller you want to use. Even if you don't, do it. 

###Process
Set the following values:

Clean Repository: False
Solution to build: Path of the solution you want to build.
Configuration to build: {enter the configuration you want} for instance „Release“
Platform to build: Any CPU or which platform you prefer.
MSBuild arguments: /p:GenerateProjectSpecificOptputFolder=False