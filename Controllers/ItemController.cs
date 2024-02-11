using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudioWebApi.Models;
using StudioWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository itemRepository;
        public ItemController(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        [Route("GetAllItems")]
        [HttpPost]
        public IActionResult GetAllItems(Item item)
        {
            var customers = itemRepository.GetAllItems(item);
            return Ok(customers);
        }

        [Route("GetItemById")]
        [HttpPost]
        public IActionResult GetItemById(Item item)
        {
            var itemData = itemRepository.GetItemById(item);

            if (itemData == null)
                return NotFound();

            return Ok(itemData);
        }

        [Route("AddItem")]
        [HttpPost]
        public IActionResult AddItem([FromBody] Item item)
        {
            itemRepository.AddItem(item);
            var unitData = itemRepository.GetItemById(item);
            return Ok(unitData);
        }

        [Route("UpdateItem")]
        [HttpPost]
        public IActionResult UpdateItem([FromBody] Item item)
        {
            var existingItem = itemRepository.GetItemById(item);

            if (existingItem == null)
                return NotFound();

            var result = itemRepository.UpdateItem(item);

            return Ok(result);
        }

        [Route("DeleteItem")]
        [HttpPost]
        public IActionResult DeleteItem(Item item)
        {
            var existingItem = itemRepository.GetItemById(item);

            if (existingItem == null)
                return NotFound();

            var result = itemRepository.DeleteItem(item);

            return Ok(result);
        }

        [Route("GetAllUnits")]
        [HttpPost]
        public IActionResult GetAllUnits(Unit unit)
        {
            var customers = itemRepository.GetAllUnits(unit);
            return Ok(customers);
        }

        [Route("GetUnitById/{id}")]
        [HttpPost]
        public IActionResult GetUnitById(Unit unit)
        {
            var unitData = itemRepository.GetUnitById(unit);

            if (unitData == null)
                return NotFound();

            return Ok(unitData);
        }

        [Route("AddUnit")]
        [HttpPost]
        public IActionResult AddUnit([FromBody] Unit unit)
        {
            itemRepository.AddUnit(unit);
            var unitData = itemRepository.GetUnitById(unit);
            return Ok(unitData);
            //return CreatedAtAction(nameof(GetUnitById), new { id = unit.Id }, unit);
        }

        [Route("UpdateUnit")]
        [HttpPost]
        public IActionResult UpdateUnit([FromBody] Unit unit)
        {
            var existingUnit = itemRepository.GetUnitById(unit);

            if (existingUnit == null)
                return NotFound();

            itemRepository.UpdateUnit(unit);
            var result = itemRepository.GetUnitById(unit);

            return Ok(result);
        }

        [Route("DeleteUnit")]
        [HttpPost]
        public IActionResult DeleteUnit(Unit unit)
        {
            var existingUnit = itemRepository.GetUnitById(unit);

            if (existingUnit == null)
                return NotFound();

            var result = itemRepository.DeleteUnit(unit);

            return Ok(result);
        }
    }
}
