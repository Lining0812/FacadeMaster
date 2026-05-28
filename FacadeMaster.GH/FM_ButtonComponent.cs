using Grasshopper.Kernel;

namespace FacadeMaster.GH
{
    public abstract class FM_ButtonComponent : GH_Component
    {
        public FM_ButtonComponent(string name, string nickname, string description, string category, string subCategory)
            : base(name, nickname, description, category, subCategory)
        {
        }

        /// <summary>
        /// Gets the display name of the button in the component UI.
        /// </summary>
        protected abstract string ButtonDisplayName { get; }
        protected bool IsButtonActivate()
        {
            if (this.Attributes is ButtonAttribute att)
            {
                var activate = att.Activate;
                return activate;
            }
            return false;
        }

        public override void CreateAttributes()
        {
            this.Attributes = new ButtonAttribute(this, ButtonDisplayName);
        }

    }
}
