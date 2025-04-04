namespace Products.Api.Services
{
    using System.Collections.Generic;
    using System.Text.Json;

    using Products.Api.Entities;
    using Products.Api.Services.Interfaces;
    using Products.Api.Utils;
    using Serilog;
    using StackExchange.Redis;

    public class CachedSubCategoryService : Logger, ISubCategoryServiceCached
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IDatabase _cache;
        private readonly Logger _logger;

        public CachedSubCategoryService(ISubCategoryRepository subCategoryRepository, IConnectionMultiplexer redis, Logger logger)
        {
            _subCategoryRepository = subCategoryRepository;
            _cache = redis.GetDatabase();
            _logger = logger;
        }


        public async Task<SubCategory> GetSubCategorybyIdAsync(Guid id, CancellationToken token)
        {
            _logger.SetInformationLogMessage("Get data for find one SubCategory ", "GetSubCategorybyIdAsync");

            
            string cacheKey = $"subcategory:{id}";
            string cachedData = await _cache.StringGetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
               _logger.SetInformationLogMessage($"Get GetSubCategorybyIdAsync chached {cachedData} from redis server ", "GetSubCategorybyIdAsync");
                return JsonSerializer.Deserialize<SubCategory>(cachedData)!;
            }
            CancellationToken ct = new CancellationToken();

            var subCategory = await _subCategoryRepository.GetSubCategorybyIdAsync(id, ct);

            if (subCategory != null)
            {
                await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(subCategory), TimeSpan.FromMinutes(5));
            }

            return subCategory;
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync(CancellationToken ct, int page = 1, int pageSize = 10, string code="")
        {
            _logger.SetInformationLogMessage("Get data for list SubCategories ", "GetSubCategoriesAsync");
            try
            {
                string cacheKey = $"subcategory_list:page:{page}:size:{pageSize}:code:{code}";
                string cachedData = await _cache.StringGetAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    _logger.SetInformationLogMessage($"Get GetSubCategoriesAsync chached {cachedData} from redis server ", "GetSubCategoriesAsync");

                    return JsonSerializer.Deserialize<List<SubCategory>>(cachedData)!;
                }

                var subCategoriesDb = await _subCategoryRepository.GetSubCategoryAsync(new CancellationToken(), page, pageSize,code);

                if (subCategoriesDb != null && subCategoriesDb.Any())
                {
                    await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(subCategoriesDb), TimeSpan.FromMinutes(5));
                }
            }
            catch (RedisConnectionException ex)
            {
                _logger.SetErrorLogMessage($"Error on Redis server exception on {ex.Message} check your redis server", "GetProductsAsync", ex.ToString());
                _logger.SetInformationLogMessage($"Get SubCategories from database ", "GetSubCategoriesAsync");

            }

            var subCategories = await _subCategoryRepository.GetSubCategoryAsync(new CancellationToken(), page, pageSize, code);

            return subCategories;
        }
    }

}
