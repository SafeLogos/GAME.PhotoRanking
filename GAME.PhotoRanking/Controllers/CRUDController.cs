using GAME.PhotoRanking.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GAME.PhotoRanking.Controllers
{
    public abstract class CRUDController<Repository, ResponseDTO, AddRequest, UpdateRequest> : ControllerBase
        where Repository : ICRUDRepository<ResponseDTO, AddRequest, UpdateRequest>
    {
        protected readonly Repository _repository;
        public CRUDController(Repository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRequest request) =>
            Ok(await  _repository.Add(request));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request) =>
            Ok(await _repository.Update(request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) =>
            Ok(await _repository.Delete(id));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) =>
            Ok(await _repository.Get(id));

        [HttpGet("all")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repository.GetAll());
    }
}
