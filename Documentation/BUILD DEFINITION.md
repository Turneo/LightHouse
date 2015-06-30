# Build Definition (LightHouse)

1 OVERVIEW

Build Definitions are used to build and test the projects in a solution. It is possible to 
select whether it should be built at a specific time or right after a commit. Each solution has 
its own Build Definitions for 32 or for 64 Bit. If the Build Definition could build the entire 
solution without complications, it is shown as completed in TFS. If it just partly succeeded it 
has an exclamation mark and if it failed there is a cross.

2 Build Server

The build server is installed on TEO-TS-MWS-001 (167.114.57.227) and is able to connect through 
TFS online (turneo.vistualstudio.com). The build controller can be selected on TFS online or on 
the build server TEO-TS-MWS-001.

3 Create Build Definition to build on TEO-TS-MWS-001
It takes the following steps to create a new build definition using visual studio:
Go to Team Explorer -> Build -> New Build Definition

The following points should be configured before saving the build Definition:

	General
	The name of the Build Definition is defined in General. The name of the Solution is also 
	the name of the Build Definition. Before the solution name there is also the type of the 
	trigger. If the Build Definition is triggered as Continuous Integration, the abbreviation CI 
	should be in front of it. If it is triggered at night, there should be a nightly in front of 
	it. There are 2 Build Definitions for each solution. One for 64-Bit (x64) and one for 32-Bit (
	x86). This is the Build Definition name for LightHouse Core for the Continuous integration: 
	CI – LightHouse Core (x86). 
	
	Trigger
	In this category you can see how to set the triggers for the Build Definition. In this example
	the Continuous Integration is selected. That means it will be built as soon as something 
	is committed.
	
	Source Settings
	In Source Settings you can set which repository should be built. 
	
	Build Defaults
	Select the Build controller „TEO-TS-MWS-001“
	
	Process
	Set the following values:
	Clean Repository: False
	Solution to build: Path of the solution you want to build (in this case \Code\Lighthouse Core.sln)
	Configuration to build: {enter the configuration you want} in this case „Release“
	Platform to build: AnyCPU
	MSBuild arguments: /p:GenerateProjectSpecificOptputFolder=False

	After saving the new build definition it appears in „Team Explorer – Builds“. 


