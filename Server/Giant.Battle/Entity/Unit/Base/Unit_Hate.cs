namespace Giant.Battle
{
    public partial class Unit
    {
        protected HateComponent HateComponent { get; private set; }

        protected virtual void InitHate() 
        {
            HateComponent = AddComponentWithParent<HateComponent>(this);
        }
    }
}
