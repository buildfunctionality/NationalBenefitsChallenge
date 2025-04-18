﻿namespace Products.Api.Services
{
    using System.Collections.Generic;
    using System.Text.Json;
    using Polly.CircuitBreaker;
    using Products.Api.Entities;
    using Products.Api.Services.Interfaces;
    using Products.Api.Utils;
    using Serilog;
    using StackExchange.Redis;

    public class CachedProductService : Logger, IProductServiceCached
    {
        private readonly IProductRepository _productRepository;
        private readonly IDatabase _cache;
        private readonly Logger _logger;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public CachedProductService(IProductRepository productRepository, IConnectionMultiplexer redis, Logger logger)
        {
            _productRepository = productRepository;
            _cache = redis.GetDatabase();
            _logger = logger;
            _circuitBreakerPolicy = DatabaseCircuitBreakerPolicy.CreatePolicy();
        }


        public async Task<Products> GetProductbyIdAsync(Guid id, CancellationToken token)
        {
            _logger.SetInformationLogMessage("Get data for find one product ", "GetProductbyIdAsync");


            string cacheKey = $"product:{id}";
            string cachedData = await _cache.StringGetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.SetInformationLogMessage($"Get GetProductbyIdAsync chached {cachedData} from redis server ", "GetSubCategorybyIdAsync");
                return JsonSerializer.Deserialize<Products>(cachedData)!;
            }
            CancellationToken ct = new CancellationToken();

            var product = await _productRepository.GetProductbyIdAsync(id, ct);

            if (product != null)
            {
                await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(product), TimeSpan.FromMinutes(5));
            }

            return product;
        }

        public async Task<IEnumerable<Products>> GetProductsAsync(CancellationToken ct, int page = 1, int pageSize = 10, string name="")
        {
            _logger.SetInformationLogMessage("Get data for list products ", "GetProductsAsync");

            try
            {
                string cacheKey = $"product_list:page:{page}:size:{pageSize}:name:{name}";
                string cachedData = await _cache.StringGetAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    _logger.SetInformationLogMessage($"Get GetProductsAsync chached {cachedData} from redis server ", "GetProductsAsync");

                    return JsonSerializer.Deserialize<List<Products>>(cachedData)!;
                }

                var productsDB = await _productRepository.GetProductsAsync(new CancellationToken(), page, pageSize,name);

                if (productsDB != null && productsDB.Any())
                {
                    await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(productsDB), TimeSpan.FromMinutes(3));
                }

            }
            catch (RedisConnectionException ex)
            {
                _logger.SetErrorLogMessage($"Error on Redis server exception on {ex.Message} check your redis server", "GetProductsAsync", ex.ToString());
                _logger.SetInformationLogMessage($"Get GetProductsAsync from database ", "GetProductsAsync");

            }

            var products = await _productRepository.GetProductsAsync(new CancellationToken(), page, pageSize,name);
          
            return products;
        }
    }

}
