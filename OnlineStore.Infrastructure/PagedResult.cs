using System.Collections;
using System.Collections.Generic;

namespace OnlineStore.Infrastructure
{
    /// <summary>
    /// 分页结果
    /// </summary>
    public class PagedResult<T> : IEnumerable<T>, ICollection<T>
    {
        public static readonly PagedResult<T> EmptyPagedResult = new PagedResult<T>(0, 0, 0, 0, null);

        #region Public Properties
        // 总记录数
        public int TotalRecords { get; set; }

        // 总页数
        public int TotalPages { get; set; }

        // 每页的记录数
        public int PageSize { get; set; }

        // 页码
        public int PageNumber { get; set; }

        /// <summary>
        /// 获取或设置当前页码的记录
        /// </summary>
        public List<T> PageData { get; set; }

        #endregion 

        #region Ctor

        public PagedResult()
        {
            this.PageData =new List<T>();
        }

        public PagedResult(int totalRecords, int totalPages, int pageSize, int pageNumber, List<T> data)
        {
            this.TotalPages = totalPages;
            this.TotalRecords = totalRecords;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.PageData = data;
        } 
        #endregion

        #region Override Object Members
        /// <summary>
        /// 确定指定的Object是否等于当前的Object。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns>如果指定的Object与当前Object相等，则返回true，否则返回false。</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == (object)null)
                return false;
            var other = obj as PagedResult<T>;
            if (ReferenceEquals(other, (object)null))
                return false;
            return this.TotalPages == other.TotalPages &&
                this.TotalRecords == other.TotalRecords &&
                this.PageNumber == other.PageNumber &&
                this.PageSize == other.PageSize &&
                this.PageData == other.PageData;
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>当前Object的哈希代码。</returns>
        public override int GetHashCode()
        {
            return this.TotalPages.GetHashCode() ^
                this.TotalRecords.GetHashCode() ^
                this.PageNumber.GetHashCode() ^
                this.PageSize.GetHashCode();
        }

        /// <summary>
        /// 确定两个对象是否相等。
        /// </summary>
        /// <param name="a">待确定的第一个对象。</param>
        /// <param name="b">待确定的另一个对象。</param>
        /// <returns>如果两者相等，则返回true，否则返回false。</returns>
        public static bool operator ==(PagedResult<T> a, PagedResult<T> b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if ((object)a == null || (object)b == null)
                return false;
            return a.Equals(b);
        }

        /// <summary>
        /// 确定两个对象是否不相等。
        /// </summary>
        /// <param name="a">待确定的第一个对象。</param>
        /// <param name="b">待确定的另一个对象。</param>
        /// <returns>如果两者不相等，则返回true，否则返回false。</returns>
        public static bool operator !=(PagedResult<T> a, PagedResult<T> b)
        {
            return !(a == b);
        }
        #endregion

        #region IEnumberable Members
        public IEnumerator<T> GetEnumerator()
        {
            return PageData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return PageData.GetEnumerator();
        }
        #endregion 
    
        #region ICollection Members
        public void Add(T item)
        {
            PageData.Add(item);
        }

        public void Clear()
        {
            PageData.Clear();
        }

        public bool Contains(T item)
        {
            return PageData.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            PageData.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return PageData.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return PageData.Remove(item);
        }
        #endregion 
    }
}