namespace BookStore.Business
{
    public enum AppErrorCode
    {
        InternalServerError = 500,

        DuplicateName = 550,

        DuplicateCode = 551,

        NameCannotBeNull = 552,

        DataTableColumnsCannotbeNull = 553,

        DataNotFound=554,

        MandatoryFieldsCannotBeNull = 555,

        InvalidInputParameter = 556,

        DataAlreadyInUse = 557

    }
}
