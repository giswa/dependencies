// Top-level statement
// get args
if (args.Length > 0){
    string pathString = args[0] ;
    //checked file path 
    bool exists = pathString.IndexOfAny(Path.GetInvalidPathChars()) == -1;

    if (exists) {
        Project p = new Project(pathString); 
        //export graph
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
        Console.WriteLine("---");
        Console.WriteLine("output graph: " + outputfile);
        Console.WriteLine("run cmd: start " + outputfile );
        

    }
} else {
    Console.WriteLine($"Project path is missing");  
}

