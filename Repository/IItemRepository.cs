using StudioWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Repository
{
    public interface IItemRepository
    {
        public Result GetAllItems(Item item);

        public Result GetItemById(Item item);

        public string AddItem(Item item);

        public Result UpdateItem(Item item);

        public Result DeleteItem(Item item);

        public Result GetAllUnits(Unit unit);

        public Result GetUnitById(Unit unit);

        public string AddUnit(Unit unit);

        public Result UpdateUnit(Unit unit);

        public Result DeleteUnit(Unit unit);
    }
}
