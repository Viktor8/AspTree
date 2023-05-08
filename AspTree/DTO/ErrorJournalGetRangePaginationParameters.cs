using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspTree.DTO
{
    public class ErrorJournalGetRangePaginationParameters
    {
        [BindRequired]
        public int Skip { get; set; }

        [BindRequired]
        public int Take { get; set; }
    }
}
