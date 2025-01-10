﻿// Top-level statement
// get args
if (args.Length > 0){
    string pathString = args[0] ;
    //checked file path 
    bool exists = pathString.IndexOfAny(Path.GetInvalidPathChars()) == -1;

    if (exists) {
        Project p = new Project(pathString,0); 
        p.Search();
        
        Console.WriteLine("");
        Console.WriteLine(p.Filename);
        Treeview(p.Dependencies,"", "") ;
        return ;
        Console.WriteLine("");
        Console.WriteLine("Creating graph");
        //export graph
         Globals.FLAT = new Dictionary<String, Project>() ;
        string text = p.Output();
        string title = p.Filename ;
        string outputfile = title + ".html" ;

        // Write output from html template
        var template = File.ReadAllText("template.html");
        var html = template
            .Replace("{{output}}", text)
            .Replace("{{title}}", title);

        File.WriteAllText(outputfile, html);

        // done
        Console.WriteLine("");
        Console.WriteLine("output graph: " + outputfile);
        Console.WriteLine("run cmd: start " + outputfile );
        

    }
} else {
    Console.WriteLine($"Project path is missing");  
}


static void Treeview(List<Project> nodes, string level, string branch ) {
    string treeLevel =   "├── " ;
    string treeEnd =     "└── " ;
    string treeOpen =    "│   " ;
    string treeClosed =  "    " ;

    if (nodes != null){

        int position = 0 ;
        int last = nodes.Count - 1 ;

        foreach(Project node in nodes) {
            bool end = position == last ;
            string treeSeparator = end ? treeEnd : treeLevel ;
            string LevelSeparator = end ? treeClosed : treeOpen ;
            
            bool circular = branch.Contains(node.Filename) ? true : false ;
            branch += $" > {node.Filename}" ;
            Console.WriteLine( level + treeSeparator +  node.Filename +  $"-->({branch})" );

            if ( circular) {
                // Console.WriteLine( level + branch + $" !> [{node.Filename}]");
            }
            else
            {
                //Console.Write("\r\n");
                // if (node.Dependencies.Count == 0 ) branch = new List<String>() ;
                Treeview(node.Dependencies, level + LevelSeparator , branch );

            }
            position ++ ;
        }
    }
}