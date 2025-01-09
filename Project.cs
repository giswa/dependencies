
using System.Xml;

public class Project{

    private string SourcePath { get; set; }

    private string BaseDirectory { get; set; }
    public string Filename { get; set; }
    private int Depth { get; set; }
    public List<Project> Dependencies {get; set;}



    public Project(string path, int depth) 
    {
        Depth = depth + 1 ;
        SourcePath = path ;
        BaseDirectory = new FileInfo(path).Directory.FullName;
        Filename = Path.GetFileName(path);  
    
    }

    public void Search(){
        Dependencies = new List<Project>();
        
		XmlDocument XmlTree = new XmlDocument();
        XmlTree.Load(SourcePath);
        // get all project reference nodes
        var nodes = XmlTree.SelectNodes("//*[local-name() = 'ProjectReference']");
		if (nodes != null)
        // loop through
            foreach (XmlNode node in nodes)
        {
            // get attribute *relative* file path (Include)
            string referencePath = node.Attributes["Include"]?.Value;
            if (!String.IsNullOrEmpty(referencePath)){
                // replace DirectorySeparatorChar. Seems to be always in Windows format in .proj files
                referencePath = referencePath.Replace(@"\", Path.DirectorySeparatorChar.ToString());
                string fullPath = Path.GetFullPath(referencePath, BaseDirectory);

                Project p;
                // if the project wasn't already parsed
                if (!Globals.FLAT.ContainsKey(fullPath))
                {
                    p = new Project(fullPath, Depth);
                    p.Search();
                    Globals.FLAT.Add(fullPath,p);
                }
                else 
                {
                    p = Globals.FLAT[fullPath];
                }
                // add to dependency
                Dependencies.Add(p);

            }
        }
    }


    public string Output(){
        string str = "" ;
        if (Dependencies?.Count > 0){
            foreach (Project dep in Dependencies){
                string link = $"{Filename} --> {dep.Filename}";
                if (!Globals.FLAT.Keys.Contains(link))
                {
                    str += link + "\n" ; 
                    Globals.FLAT.Add(link,null); //no need to store the project
                }
                str += dep.Output();
            }
        }
        return str ;
    }

}