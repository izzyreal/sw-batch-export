"# sw-batch-export" 

There's a lot of hardcoded stuff in this add-in that makes it unready for generic use. But if you ever need to automate exporting individual components from your SolidWorks assembly to STL or other file formats, this code might come in handy. File names for each component are supplied via a comment on your component.

I used this video to set up the project in VS2017: https://www.youtube.com/watch?v=EdzDd_Lx5gA

VS2017 needs the .NET desktop development workload to be installed. In the project wizard I selected Visual C# > Windows Classic Desktop > Class Library (.NET Framework).
