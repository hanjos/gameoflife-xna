My first shot at using XNA. It works, but it's ugly as sin (well, less ugly now that I worked on it some :), and will suffer several refactorings as I learn XNA and C# idioms.

How to use
----------

Get Visual Studio (I used [Visual C# 2010 Express][vcsx10], it's free) and [XNA Game Studio 4.0][xna4], add the solution, and build it. A binary will be created somewhere inside the project. Dunno if there's a better way to do things in the .NET world.

Dependencies
------------

* [Visual C# 2010 Express][vcsx10]
* [XNA Game Studio 4.0][xna4]
* [NUnit 2.5.10.11092][nunit2_5_10] for testing

Versions
--------

The tag messages should tell you everything, but, at the risk of redundancy:

* **0.7**: able to draw grid lines and clear the world; magic values moved to the configuration file.
* **0.6**: using a configuration file instead of hardcoded values.
* **0.5**: input can now be configured (programmatically).
* **0.4**: the input component was getting coupled to the other components, so its events are gone, and the game state component is the foundation component.
* **0.3**: divvied up MainGame's logic in XNA game components ([thanks][nuclex-components]).
* **0.2**: tried using events and some extra abstractions here and there for input processing, since XNA provides little.
* **0.1**: the first working version.

Working on
----------

* **Testing**. Kinda silly for a small project like this, but it's a good excuse to learn :). I have been delaying this one for a while, but now seems like a good time to shore up the code, since the other stuff will probably lead to some breakage. 

To do
-----

Expanded in the wiki page [To Be Continued][tobecontinued]


Documentation
-------------

What do you mean documentation, my code isn't good enough for you? :P

[vcsx10]: http://www.microsoft.com/visualstudio/en-us/products/2010-editions/visual-csharp-express
[xna4]: http://www.microsoft.com/download/en/details.aspx?id=23714
[nunit2_5_10]: http://www.nunit.org/download.html
[nuclex-components]: http://www.nuclex.org/articles/architecture/6-game-components-and-game-services
[ninject]: http://ninject.org/
[ninject-xna]: http://www.nuclex.org/articles/architecture/9-using-dependency-injection-in-xna
[nuget]: http://nuget.org/
[tobecontinued]: https://github.com/hanjos/gameoflife-xna/wiki/To-Be-Continued