using AutoMapper;
using ChatSignalRR.Data;
using ChatSignalRR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalRR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ChatContext _context;
        private readonly IMapper _mapper;
      
        public RoomsController(ChatContext context,
          IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
          
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomViewModel>>> Get()
        {

            var rooms = await _context.Rooms.ToListAsync();

            var roomsViewModel = _mapper.Map<IEnumerable<Room>, IEnumerable<RoomViewModel>>(rooms);

            return Ok(roomsViewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> Get(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound();

            var roomViewModel = _mapper.Map<Room, RoomViewModel>(room);
            return Ok(roomViewModel);
        }




    }
}
