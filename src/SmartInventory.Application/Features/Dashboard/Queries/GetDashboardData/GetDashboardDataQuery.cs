using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Dashboard.Queries.GetDashboardData;

public class GetDashboardDataQuery : IRequest<ApiResponse<DashboardDto>>
{
}

