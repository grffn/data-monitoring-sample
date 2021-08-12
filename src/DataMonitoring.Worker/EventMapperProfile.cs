using AutoMapper;
using DataMonitoring.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMonitoring.Worker
{
    class EventMapperProfile: Profile
    {
        public EventMapperProfile()
        {
            CreateMap<AssetPositionEvent, AssetPosition>();
        }
    }
}
