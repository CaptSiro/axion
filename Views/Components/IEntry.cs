namespace axion.Views.Components;

public interface IEntry
{
    public string EntryName { get; set; }

    public string EntryPath { get; set; }



    public void Rename(string name);
}