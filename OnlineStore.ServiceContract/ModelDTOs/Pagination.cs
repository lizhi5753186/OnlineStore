namespace OnlineStore.ServiceContracts.ModelDTOs
{
    // 分页信息类
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TotalPages { get; set; }

        public override string ToString()
        {
            return string.Format("PageSize={0} PageNumber={1} TotalPages={2}",
                this.PageSize,
                this.PageNumber,
                this.TotalPages ?? 0);
        }
    }
}