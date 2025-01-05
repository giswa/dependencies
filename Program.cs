// Top-level statement
// get args
if (args.Length > 0){
    string pathString = args[0] ;
    //checked file path 
    bool exists = pathString.IndexOfAny(Path.GetInvalidPathChars()) == -1;

    if (exists) {
        Project p = new Project(pathString); 
        //export graph
        p.Output();
    }
} else {
    Console.WriteLine($"Project path is missing");  
}

