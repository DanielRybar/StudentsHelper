namespace StudentsHelper.Helpers
{
    public static class CollectionViewExtension
    {
        public async static Task ResetAnimation(this CollectionView collectionView)
        {
            if (collectionView is null)
            {
                return;
            }
            var vsTreeDescendants = collectionView.GetVisualTreeDescendants();
            if (vsTreeDescendants is not null)
            {
                foreach (var item in vsTreeDescendants)
                {
                    var rootViews = item.GetVisualChildren();
                    if (rootViews is not null)
                    {
                        foreach (var view in rootViews)
                        {
                            if (view is Grid grid)
                            {
                                await grid.ScaleTo(1, 100);
                            }
                        }
                    }
                }
            }
        }
    }
}