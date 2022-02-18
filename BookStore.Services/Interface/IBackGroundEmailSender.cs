using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Interface
{
    public interface IBackGroundEmailSender
    {
        Task DoWork();
    }
}
