// This file defines the ComponentService class, which provides methods for retrieving and managing computer components and their reviews from the database.

using ComputerBuilderMvcApp.Data;
using ComputerBuilderMvcApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerBuilderMvcApp.Services
{
    public interface IComponentService
    {
        Task<List<Component>> GetComponentsAsync(List<string> categories);
        Task<List<Component>> GetFeaturedComponentsAsync(int count, List<string> categories);
        Task<List<Component?>> GetReviewsForComponentIdAsync(int componentId);
        Task<Component?> GetComponentByIdAsync(int componentId);
    }

    public class ComponentService(ApplicationDbContext context) : IComponentService
    {
        private readonly ApplicationDbContext _context = context;

        // Retrieves all components, optionally filtered by a list of categories.
        public async Task<List<Component>> GetComponentsAsync(List<string> categories)
        {
            IQueryable<Component> componentsQuery = _context.Component.Include(c => c.Reviews);
            if (categories != null && categories.Count != 0)
            {
                var lowerCategoriesToFilter = categories.Select(c => c.ToLowerInvariant()).ToList();
                componentsQuery = componentsQuery.Where(c => c.Type != null && lowerCategoriesToFilter.Contains(c.Type.ToLowerInvariant()));
            }
            return await componentsQuery.ToListAsync();
        }

        // Retrieves a random selection of featured components, filtered by categories if provided.
        public async Task<List<Component>> GetFeaturedComponentsAsync(int count, List<string> categories)
        {
            var allComponents = await GetComponentsAsync(categories);
            var random = new Random();
            return [.. allComponents.OrderBy(c => random.Next()).Take(Math.Min(count, allComponents.Count))];
        }

        // Retrieves a component and its reviews by component ID.
        public async Task<List<Component?>> GetReviewsForComponentIdAsync(int componentId)
        {
            var component = await _context.Component.Include(c => c.Reviews).FirstOrDefaultAsync(c => c.Id == componentId);
            return component != null ? [component] : [];
        }

        // Retrieves a single component by its ID.
        public async Task<Component?> GetComponentByIdAsync(int componentId)
        {
            return await _context.Component.FirstOrDefaultAsync(c => c.Id == componentId);
        }
    }
}