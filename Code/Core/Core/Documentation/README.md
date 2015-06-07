# LightHouse Core

Portable core components of LightHouse, composed by the following elements:

## Model 

The Model in LightHouse is composed by Data, Contract and Surrogate Objects.

1. Data Objects: contain the model used by one installation of a LightHouse application.
2. Contract Objects: contain the model used by one or more installations.
3. Surrogate Objects: represent a Data or Contract Object in specific situations (can be used as a DTO).

## Caching

A thread safe portable cache that supports regions.

## Collections

Several lists to support the base model types of LightHouse. Including support for enumeration and LINQ queries.

## Building

Functionality for building different types of objects.

## Cloning

Functionality for building DataObjects.

## Converting

Functionality for converting different types of objects. Currently includes conversion between DataObjects, SurrogateObjects and ContractObjects.

## Loading

Functionality for loading proxied DataObjects from the corresponding data source.

## Locating

Functionality for locating types and type informations.

## Merging

Functionality for merging objects.

## Notifying

Functionality for notifying events.

## Reflecting

Functionality for getting information about types and assemblies. The Reflector tries to improve the performance of System.Reflection. If that isn't possible the standard functionality of the .NET Framework is used.

NuGet Package:

https://www.nuget.org/packages/LightHouse.Core/

LightHouse is a registered trademark of Turneo AG.
