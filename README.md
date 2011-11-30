My first shot at using XNA. It works, but it's ugly as sin, and will suffer several refactorings as I learn XNA and C# idioms.

How to use
----------

Get Visual Studio (I used [Visual C# 2010 Express][vcsx10], it's free) and [XNA Game Studio 4.0][xna4], add the solution, and build it. A binary will be created somewhere inside the project. Dunno if there's a better way to do things in the .NET world.

To do
-----

In no particular order:

* **Testing**. Kinda silly for a small project like this, but it's a good excuse to learn how .NET folks do stuff.
* **Property files**: Lots of magic numbers and values which could be put in an external script; gotta learn how first...
* **Console**: A console seems like a nice idea :)
* **WinForms integration**: Apparently XNA's standard usage doesn't match up well with WinForms; must investigate further.
* **Dependency injection**: Worth a [look][ninject-xna], if only to rule it out. Seems like [Ninject][ninject] is the name of the game.
* **Dependency management**: [Nuget][nuget]? Is there enough widespread usage, like Maven?
* **Refactoring**: Basically prettying up the code. It's already been componentized ([thanks, by the way][nuclex-components]), but some touches here and there are always nice.
* **Distribution**: Asking people to run Visual Studio just to get a binary isn't nice :)

Documentation
-------------

What do you mean documentation, my code isn't good enough for you? :P

[vcsx10]: http://www.microsoft.com/visualstudio/en-us/products/2010-editions/visual-csharp-express
[xna4]: http://www.microsoft.com/download/en/details.aspx?id=23714
[nuclex-components]: http://www.nuclex.org/articles/architecture/6-game-components-and-game-services
[ninject]: http://ninject.org/
[ninject-xna]: http://www.nuclex.org/articles/architecture/9-using-dependency-injection-in-xna
[nuget]: http://nuget.org/