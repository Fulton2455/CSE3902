// Connor Fulton
namespace _3902sprint0
{
    
    public class MenuButton
    {
        public string Label { get; }
        public MenuAction Action { get; }

        public MenuButton(string label, MenuAction action)
        {
            Label = label;
            Action = action;
        }
    }
}