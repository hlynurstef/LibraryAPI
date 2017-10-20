# LibraryAPI

Library API in .NET Core

## Students

* Andri Ívarsson (andrii13@ru.is)
* Gunnar Davíð Gunnarsson (gunnarg15@ru.is)
* Guðjón Steinar Sverrisson (gudjonss12@ru.is)
* Hlynur Stefánsson (hlynurs15@ru.is)

# Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

## Install .NET Core

First you have to make sure you have .NET Core installed on your machine, go to the following link and install it:

https://www.microsoft.com/net/core

## Visual Studio Code - Extensions

If you are using VSCode we recommend the following extensions:

* C# for Visual Studio Code (powered by OmniSharp)
    * Provides IntelliSense features, syntax highlighting and etc.
* C# Extensions
    * Provides features such as 'Add new C# class', 'Add new interface' when right-clicking the folder tree
* C# XML Documentation Comments
    * Auto-generation of XML comments
* C# Snippets
* TODO Highlights
* NuGet Package Manager
    * Simplifies adding NuGet packages to projects and/or solutions
    * Documentation: https://github.com/jmrog/vscode-nuget-package-manager/blob/master/README.md (Links to an external site.)Links to an external site. 
* .NET Core Test Explorer
    * Provides a Test Explorer inside Visual Studio Code

## Sending requests to the API

To send requests to the API we recommend installing Postman:

https://www.getpostman.com/

## Browsing the database

To be able to browse the database we recommend installing Db Browser for SQLite:

http://sqlitebrowser.org/

# Running the project

Once you've installed .NET Core you can clone this repo to your local machine. Then you have to make sure that you've run the restore command on all of the folders. To do this navigate to each folder in your command line and run "dotnet restore" in each folder:

```
cd Api/
dotnet restore

cd ../Models/
dotnet restore

cd ../Services/
dotnet restore

cd ../Repositories/
dotnet restore
```

Once you've made sure that you've run restore on all the folders then you can try running the project. Before you can run it you need to build it, to do that navigate to the Api folder and run "dotnet build" there:

```
cd Api/
dotnet build
```

Now you should be able to run the project. Make sure you are still in the Api folder and run "dotnet run":

```
dotnet run
```

At this point the Api should be up and running on your localhost on port 5000. To test the Api you can, for example, open up Postman and try sending a GET request to http://localhost:5000/books and you should see a list of books returned.

# Running the tests

Explain how to run the automated tests for this system

## Built With

* [.Net Core](https://www.microsoft.com/net/core) - The web framework used
