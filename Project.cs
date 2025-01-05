
using System.Xml;

public class Project{

    private string SourcePath { get; set; }
    private XmlDocument XmlTree { get; set; }
    private string BaseDirectory { get; set; }
    private string Filename { get; set; }
    public List<Project> Dependencies {get; set;}

    public Project(string path){
        SourcePath = path ;
        BaseDirectory = new FileInfo(path).Directory.FullName;
        Filename = Path.GetFileName(path);
    }

    public void Search(){
        Dependencies = new List<Project>();
        
        XmlTree = new XmlDocument();
        XmlTree.Load(SourcePath);
        // get all project reference nodes
        var projectReferenceNodes = XmlTree.SelectNodes("//*[local-name() = 'ProjectReference']");
        // loop through
        foreach (XmlNode node in projectReferenceNodes)
        {
            // get attribute *relative* file path (Include)
            string referencePath = node.Attributes["Include"]?.Value;
            if (!String.IsNullOrEmpty( referencePath)){
                // replace DirectorySeparatorChar. Seems to be always in Windows format in .proj files
                referencePath = referencePath.Replace(@"\", Path.DirectorySeparatorChar.ToString());
                string fullPath = Path.GetFullPath(referencePath, BaseDirectory);
                Project p = new Project(fullPath);
                // add to dependency
                Dependencies.Add(p);
                // recursive search sub-dependencies
                p.Search();
            }
        }

    }


    public void  Output(){

        foreach (Project dep in Dependencies){
            Console.WriteLine($"{Filename} --> {dep.Filename}");

        } 
    }

}