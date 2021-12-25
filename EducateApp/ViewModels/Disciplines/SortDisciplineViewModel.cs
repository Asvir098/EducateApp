namespace EducateApp.ViewModels.Disciplines
{
    public class SortDisciplineViewModel
    {
        public DisciplinesSortState IndexProfModuleSort { get; private set; }
        public DisciplinesSortState ProfModuleSort { get; private set; }
        public DisciplinesSortState IndexSort { get; private set; }
        public DisciplinesSortState NameSort { get; private set; }
        public DisciplinesSortState ShortNameSort { get; private set; }
        public DisciplinesSortState Current { get; private set; }

        public SortDisciplineViewModel(DisciplinesSortState sortOrder)
        {
            IndexProfModuleSort = sortOrder == DisciplinesSortState.IndexProfModuleAsc ?
                DisciplinesSortState.IndexProfModuleDesc : DisciplinesSortState.IndexProfModuleAsc;

            ProfModuleSort = sortOrder == DisciplinesSortState.ProfModuleAsc ?
                DisciplinesSortState.ProfModuleDesc : DisciplinesSortState.ProfModuleAsc;

            IndexSort = sortOrder == DisciplinesSortState.IndexAsc ?
                DisciplinesSortState.IndexDesc : DisciplinesSortState.IndexAsc;

            NameSort = sortOrder == DisciplinesSortState.NameAsc ?
                DisciplinesSortState.NameDesc : DisciplinesSortState.NameAsc;

            ShortNameSort = sortOrder == DisciplinesSortState.ShortNameAsc ?
                DisciplinesSortState.ShortNameDesc : DisciplinesSortState.ShortNameAsc;
            Current = sortOrder;
        }
    }
}
