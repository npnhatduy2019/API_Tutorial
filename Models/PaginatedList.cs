namespace API_Tutorial.Models
{
    public class PaginatedList<T>:List<T>
    {
        public int PageIndex { get; set; }  
        public int TotalPage { get; set; }

        public PaginatedList(List<T> Items,int count,int _pageindex,int _pagesize)
        {
            PageIndex=_pageindex;
            TotalPage=(int)Math.Ceiling(count/(double)_pagesize);
            AddRange(Items);
        }

        public static PaginatedList<T> Create(IQueryable<T> source,int pagesize,int pageindex)
        {
            int count=source.Count();
            var items=source.Skip((pageindex-1)*pagesize).Take(pagesize).ToList();

            return new PaginatedList<T>(items,count,pageindex,pagesize);  
        }
    }
}