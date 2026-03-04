using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Objects.CostObjects;
using DistribuidoraX.Domain.Objects.ProfitObjects;
using System.Drawing;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductUseCases
{
    internal class CalculateProductPriceUseCase : ICalculateProductPriceUseCase
    {
        private decimal productPriceResponse;
        private readonly FixedCostsPercentsObject fixedCostsPercent;
        private decimal sumProductSupplierCost;
        private readonly ProfitsPercentsObject profitsPercents;
        private readonly IProductSupplierRepository_StoredProceduresSqlServer _productSupplierRepositoryStoredProceduresSqlServer;
        public CalculateProductPriceUseCase(IProductSupplierRepository_StoredProceduresSqlServer productSupplierRepositoryStoredProceduresSqlServer)
        {
            productPriceResponse = decimal.Zero;
            fixedCostsPercent = new();
            sumProductSupplierCost = decimal.Zero;
            profitsPercents = new();
            _productSupplierRepositoryStoredProceduresSqlServer = productSupplierRepositoryStoredProceduresSqlServer;
        }

        public async Task<decimal> SimpleCalculation_ProductPriceAsync(int productId)
        {
            var productSupplierCostList = await _productSupplierRepositoryStoredProceduresSqlServer.GetCostListByProductIdAsync(productId);
            if (productSupplierCostList.Count > 0)
            {
                var sumfixedCostsPercents = fixedCostsPercent.SalaryCost_PercentAppliedForUnit +
                                        fixedCostsPercent.ElectricalEnery_PercentAppliedForUnit +
                                        fixedCostsPercent.WaterService_PercentAppliedForUnit +
                                        fixedCostsPercent.InternetService_PercentAppliedForUnit;

                foreach (var productSupplierCost in productSupplierCostList)
                {
                    sumProductSupplierCost += productSupplierCost;
                }

                var productSupplierCostTotal = sumProductSupplierCost/productSupplierCostList.Count;
                var qtyfixedCost = productSupplierCostTotal * (decimal)sumfixedCostsPercents/100;
                var totalCost = productSupplierCostTotal + qtyfixedCost;

                /*
                 * Calculate price with this simple formula
                 * precio= cost*(100/100-Profif)
                 */

                decimal profit = (decimal)100 / (100 - profitsPercents.GenericProfit_PercentAppliedForUnit);
                productPriceResponse = totalCost * profit;

            }

            return Math.Round(productPriceResponse, 2);
        }
    }
}
