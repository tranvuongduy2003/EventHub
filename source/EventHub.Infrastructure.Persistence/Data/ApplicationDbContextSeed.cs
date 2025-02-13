using System.Globalization;
using Bogus;
using EventHub.Domain.Aggregates.CouponAggregate.ValueObjects;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.Entities;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.Shared.Enums.Event;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Stripe;

namespace EventHub.Infrastructure.Persistence.Data;

public class ApplicationDbContextSeed
{
    private const int MAX_EVENTS_QUANTITY = 1000;
    private const int MAX_CATEGORIES_QUANTITY = 20;
    private const int MAX_USERS_QUANTITY = 10;

    private readonly ApplicationDbContext _context;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public ApplicationDbContextSeed(ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Seed()
    {
        await SeedRoles();
        await SeedUsers();
        await SeedFunctions();
        await SeedCommands();
        await SeedPermission();
        await SeedCategories();
        await SeedCoupons();
        await SeedEvents();
        await SeedReviews();
        await SeedFavouriteEvents();
        await SeedFollowers();
        await SeedExpenses();
    }

    private async Task SeedRoles()
    {
        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new Role(EUserRole.ADMIN.GetDisplayName()));
            await _roleManager.CreateAsync(new Role(EUserRole.CUSTOMER.GetDisplayName()));
            await _roleManager.CreateAsync(new Role(EUserRole.ORGANIZER.GetDisplayName()));
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedUsers()
    {
        if (!_userManager.Users.Any())
        {
            var admin = new User
            {
                Email = "admin@gmail.com",
                UserName = "admin",
                PhoneNumber = "0829440357",
                FullName = "Admin",
                Dob = new Faker().Person.DateOfBirth,
                Gender = EGender.MALE,
                Bio = new Faker().Lorem.Paragraph(),
                Status = EUserStatus.ACTIVE
            };
            IdentityResult result = await _userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
            {
                User user = await _userManager.FindByEmailAsync("admin@gmail.com");
                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, EUserRole.ADMIN.GetDisplayName());
                }
            }


            Faker<User> userFaker = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("###-###-####"))
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Dob, f => f.Person.DateOfBirth)
                .RuleFor(u => u.Gender, f => f.PickRandom<EGender>())
                .RuleFor(u => u.Bio, f => f.Lorem.Paragraph())
                .RuleFor(u => u.Status, _ => EUserStatus.ACTIVE);

            for (int userIndex = 0; userIndex < MAX_USERS_QUANTITY * 2; userIndex++)
            {
                User customer = userFaker.Generate();
                IdentityResult customerResult = await _userManager.CreateAsync(customer, "User@123");
                if (!customerResult.Succeeded || customer.Email == null)
                {
                    continue;
                }

                User user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    await _userManager.AddToRolesAsync(user, new List<string>
                        { EUserRole.CUSTOMER.GetDisplayName(), EUserRole.ORGANIZER.GetDisplayName() });
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedFunctions()
    {
        if (!_context.Functions.Any())
        {
            _context.Functions.AddRange(new List<Function>
            {
                new()
                {
                    Id = EFunctionCode.GENERAL.GetDisplayName(), Name = "General", ParentId = null, SortOrder = 0,
                    Url = "/"
                },

                new()
                {
                    Id = EFunctionCode.GENERAL_CATEGORY.GetDisplayName(), Name = "Categories",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/categories"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_EVENT.GetDisplayName(), Name = "Events",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/events"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_REVIEW.GetDisplayName(), Name = "Reviews",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/reviews"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(), Name = "Expenses",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/expenses"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_TICKET.GetDisplayName(), Name = "Tickets",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/tickets"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_CHAT.GetDisplayName(), Name = "Chats",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/chats"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), Name = "Payments",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/payments"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_COUPON.GetDisplayName(), Name = "Coupons",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/coupons"
                },

                new()
                {
                    Id = EFunctionCode.ADMINISTRATION.GetDisplayName(), Name = "System", ParentId = null, SortOrder = 0,
                    Url = "/administration"
                },

                new()
                {
                    Id = EFunctionCode.ADMINISTRATION_USER.GetDisplayName(), Name = "Users",
                    ParentId = EFunctionCode.ADMINISTRATION.GetDisplayName(), SortOrder = 1, Url = "/administration/users"
                },
                new()
                {
                    Id = EFunctionCode.ADMINISTRATION_ROLE.GetDisplayName(), Name = "Roles",
                    ParentId = EFunctionCode.ADMINISTRATION.GetDisplayName(), SortOrder = 1, Url = "/administration/roles"
                },
                new()
                {
                    Id = EFunctionCode.ADMINISTRATION_FUNCTION.GetDisplayName(), Name = "Functions",
                    ParentId = EFunctionCode.ADMINISTRATION.GetDisplayName(), SortOrder = 1, Url = "/administration/functions"
                },
                new()
                {
                    Id = EFunctionCode.ADMINISTRATION_PERMISSION.GetDisplayName(), Name = "Permissions",
                    ParentId = EFunctionCode.ADMINISTRATION.GetDisplayName(), SortOrder = 1, Url = "/administration/permissions"
                },
                new()
                {
                    Id = EFunctionCode.ADMINISTRATION_COMMAND.GetDisplayName(), Name = "Commands",
                    ParentId = EFunctionCode.ADMINISTRATION.GetDisplayName(), SortOrder = 1, Url = "/administration/commands"
                },
            });

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCommands()
    {
        if (!_context.Commands.Any())
        {
            _context.Commands.AddRange(new List<Command>
            {
                new() { Id = "VIEW", Name = "View" },
                new() { Id = "CREATE", Name = "Create" },
                new() { Id = "UPDATE", Name = "Update" },
                new() { Id = "DELETE", Name = "Delete" },
            });

            await _context.SaveChangesAsync();
        }

        if (!_context.CommandInFunctions.Any())
        {
            List<Function> functions = await _context.Functions.ToListAsync();

            foreach (Function function in functions)
            {
                var createAction = new CommandInFunction
                {
                    CommandId = "CREATE",
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(createAction);

                var updateAction = new CommandInFunction
                {
                    CommandId = "UPDATE",
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(updateAction);
                var deleteAction = new CommandInFunction
                {
                    CommandId = "DELETE",
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(deleteAction);

                var viewAction = new CommandInFunction
                {
                    CommandId = "VIEW",
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(viewAction);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedPermission()
    {
        if (!_context.Permissions.Any())
        {
            List<Function> functions = await _context.Functions.ToListAsync();
            Role adminRole = await _roleManager.FindByNameAsync(EUserRole.ADMIN.GetDisplayName());
            foreach (Function function in functions)
            {
                if (adminRole == null)
                {
                    continue;
                }

                _context.Permissions.Add(new Permission
                { FunctionId = function.Id, RoleId = adminRole.Id, CommandId = "CREATE" });
                _context.Permissions.Add(new Permission
                { FunctionId = function.Id, RoleId = adminRole.Id, CommandId = "UPDATE" });
                _context.Permissions.Add(new Permission
                { FunctionId = function.Id, RoleId = adminRole.Id, CommandId = "DELETE" });
                _context.Permissions.Add(new Permission
                { FunctionId = function.Id, RoleId = adminRole.Id, CommandId = "VIEW" });
            }

            Role customerRole = await _roleManager.FindByNameAsync(EUserRole.CUSTOMER.GetDisplayName());
            if (customerRole != null)
            {
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CATEGORY.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EVENT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_TICKET.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_REVIEW.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_REVIEW.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_REVIEW.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_REVIEW.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CHAT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CHAT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CHAT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CHAT.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_COUPON.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_COUPON.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_COUPON.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_COUPON.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.ADMINISTRATION.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.ADMINISTRATION.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.ADMINISTRATION_USER.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.ADMINISTRATION_USER.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
            }

            Role organizerRole = await _roleManager.FindByNameAsync(EUserRole.ORGANIZER.GetDisplayName());
            if (organizerRole != null)
            {
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CATEGORY.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EVENT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EVENT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EVENT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EVENT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_TICKET.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_TICKET.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_TICKET.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_TICKET.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_REVIEW.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_REVIEW.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_EXPENSE.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CHAT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CHAT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CHAT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_CHAT.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_COUPON.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "CREATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_COUPON.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_COUPON.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "DELETE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.GENERAL_COUPON.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.ADMINISTRATION_USER.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.ADMINISTRATION_USER.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "UPDATE"
                });
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCategories()
    {
        if (!_context.Categories.Any())
        {
            #region Categories Data

            // WARNING: Must not change the items' order, just add more!
            var categoryNames = new List<string>
            {
                "Academic", "Anniversary", "Charities", "Community", "Concerts", "Conferences", "Fashion",
                "Festivals & Fairs", "Film", "Food & Drink", "Holidays", "Kids & Family", "Lectures & Books", "Music",
                "Nightlife", "Other", "Performing Arts", "Politics", "Sports & Active Life", "Visual Arts"
            };

            var icons = new List<string>
            {
                "academic.png",
                "anniversary.png",
                "charities.png",
                "communities.png",
                "concerts.png",
                "conferences.png",
                "fashion.png",
                "festivals-and-fairs.png",
                "film.png",
                "food-and-drink.png",
                "holidays.png",
                "kids-and-family.png",
                "lectures-and-books.png",
                "music.png",
                "nightlife.png",
                "other.png",
                "performing-arts.png",
                "politics.png",
                "sports-and-active-life.png",
                "visual-arts.png"
            };

            #endregion

            Faker<Category> fakerCategory = new Faker<Category>()
                .RuleFor(c => c.Color, f => f.Commerce.Color());

            var categories = new List<Category>();
            for (int i = 0; i < MAX_CATEGORIES_QUANTITY; i++)
            {
                fakerCategory
                    .RuleFor(c => c.Name, _ => categoryNames[i])
                    .RuleFor(c => c.IconImageFileName, _ => icons[i]);

                Category category = fakerCategory.Generate();
                categories.Add(category);
            }

            _context.Categories.AddRange(categories);

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCoupons()
    {
        if (!_context.Coupons.Any())
        {
            var users = _userManager.Users.AsNoTracking().ToList();

            Faker<Domain.Aggregates.CouponAggregate.Coupon> couponFaker = new Faker<Domain.Aggregates.CouponAggregate.Coupon>()
                .RuleFor(x => x.Name, f => f.Commerce.ProductAdjective())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.AuthorId, f => f.PickRandom<User>(users).Id)
                .RuleFor(x => x.MinPrice, f =>
                    long.Parse(f.Commerce.Price(100000, 1000000, 0, ""), CultureInfo.InvariantCulture))
                .RuleFor(x => x.CoverImageUrl, f => f.Image.Random.ToString())
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10000))
                .RuleFor(x => x.PercentValue, f => f.Random.Int(20, 100))
                .RuleFor(x => x.ExpiredDate, f => DateTime.Now.AddDays(f.Random.Int(1, 1000)));

            List<Domain.Aggregates.CouponAggregate.Coupon> coupons = couponFaker.Generate(1000);

            foreach (Domain.Aggregates.CouponAggregate.Coupon coupon in coupons)
            {
                var couponOptions = new CouponCreateOptions
                {
                    Duration = "once",
                    PercentOff = (decimal)coupon.PercentValue,
                    Currency = "vnd",
                    Name = coupon.Name,
                    RedeemBy = coupon.ExpiredDate
                };

                var service = new CouponService();
                Stripe.Coupon stripeCoupone = await service.CreateAsync(couponOptions);

                coupon.Code = stripeCoupone.Id;
            }

            await _context.AddRangeAsync(coupons);
            await _context.SaveChangesAsync();
        }

    }

    private async Task SeedEvents()
    {
        if (!_context.Events.Any())
        {
            List<User> users = await _userManager.Users.ToListAsync();
            List<Category> categories = await _context.Categories.ToListAsync();
            List<Domain.Aggregates.CouponAggregate.Coupon> coupons = await _context.Coupons.ToListAsync();

            Faker<Domain.Aggregates.EventAggregate.Event> fakerEvent = new Faker<Domain.Aggregates.EventAggregate.Event>()
                .RuleFor(e => e.AuthorId, f => f.PickRandom<User>(users).Id)
                .RuleFor(e => e.Name, f => f.Commerce.ProductName())
                .RuleFor(e => e.Description, f => f.Commerce.ProductDescription())
                .RuleFor(e => e.Location, f => f.Address.FullAddress())
                .RuleFor(e => e.EventPaymentType, f => f.Random.Enum<EEventPaymentType>())
                .RuleFor(e => e.EventCycleType, f => f.Random.Enum<EEventCycleType>());

            Faker<TicketType> fakerTicketType = new Faker<TicketType>()
                .RuleFor(t => t.Name, f => f.Commerce.ProductMaterial())
                .RuleFor(t => t.Quantity, f => f.Random.Int(0, 1000))
                .RuleFor(t => t.Price, f => f.Random.Long(0, 100000000));

            Faker<EmailContent> fakerEmailContent = new Faker<EmailContent>()
                .RuleFor(e => e.Content,
                    _ =>
                        "<p>Dear attendee,<br>Thank you for attending our event! We hope you found it informative and enjoyable<br>Best regards,<br>EventHub</p>");

            Faker<Reason> fakerReason = new Faker<Reason>()
                .RuleFor(t => t.Name, f => f.Commerce.ProductDescription());

            Faker<EventCategory> fakerEventCategory = new Faker<EventCategory>()
                .RuleFor(ec => ec.CategoryId, f => f.PickRandom<Category>(categories).Id);

            for (int eventIndex = 0; eventIndex < MAX_EVENTS_QUANTITY; eventIndex++)
            {
                #region EventTime

                DateTime eventStartTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(new Faker().Random.Number(0, 60)));
                DateTime eventEndTime = DateTime.UtcNow.Add(TimeSpan.FromDays(new Faker().Random.Number(1, 60)));
                Domain.Aggregates.EventAggregate.Event eventItem = fakerEvent.Generate();
                eventItem.StartTime = eventStartTime;
                eventItem.EndTime = eventEndTime;
                if (eventItem.StartTime <= DateTime.UtcNow && DateTime.UtcNow <= eventItem.EndTime)
                {
                    eventItem.Status = EEventStatus.OPENING;
                }
                else if (DateTime.UtcNow < eventItem.StartTime)
                {
                    eventItem.Status = EEventStatus.UPCOMING;
                }
                else
                {
                    eventItem.Status = EEventStatus.CLOSED;
                }

                #endregion

                #region CoverImage

                eventItem.CoverImageUrl = new Faker().Image.PicsumUrl();

                #endregion

                _context.Events.Add(eventItem);

                #region EmailContent

                fakerEmailContent.RuleFor(ec => ec.EventId, _ => eventItem.Id);
                EmailContent emailContent = fakerEmailContent.Generate();
                _context.EmailContents.Add(emailContent);

                #endregion

                #region EventCategories

                fakerEventCategory.RuleFor(ec => ec.EventId, _ => eventItem.Id);
                IEnumerable<EventCategory> eventCategories =
                    fakerEventCategory.GenerateBetween(1, 3).DistinctBy(ec => ec.CategoryId);
                _context.EventCategories.AddRange(eventCategories.ToList());

                #endregion

                #region EventSubImages

                List<EventSubImage> subImages = new Faker<EventSubImage>().Generate(5);
                subImages.ForEach(image =>
                {
                    var eventSubImage = new EventSubImage
                    {
                        EventId = eventItem.Id,
                        ImageUrl = new Faker().Image.PicsumUrl(),
                        ImageFileName = string.Empty
                    };
                    _context.EventSubImages.Add(eventSubImage);
                });

                #endregion

                #region TicketTypes

                fakerTicketType.RuleFor(t => t.EventId, _ => eventItem.Id);
                List<TicketType> ticketTypes = fakerTicketType.Generate(3);
                _context.TicketTypes.AddRange(ticketTypes);

                #endregion

                #region Reasons

                fakerReason.RuleFor(t => t.EventId, _ => eventItem.Id);
                List<Reason> reasons = fakerReason.Generate(3);
                _context.Reasons.AddRange(reasons);

                #endregion

                #region Coupons

                Faker<EventCoupon> eventCouponFaker = new Faker<EventCoupon>()
                    .RuleFor(x => x.EventId, _ => eventItem.Id)
                    .RuleFor(x => x.CouponId, f => f.PickRandom<Domain.Aggregates.CouponAggregate.Coupon>(coupons).Id);
                var eventCoupons = eventCouponFaker.Generate(1).DistinctBy(x => x.EventId).ToList();
                _context.EventCoupons.AddRange(eventCoupons);

                #endregion

                #region Author
                User author = users.Find(x => x.Id == eventItem.AuthorId);
                author!.NumberOfCreatedEvents++;
                await _userManager.UpdateAsync(author);
                #endregion
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedFollowers()
    {
        if (!_context.UserFollowers.Any())
        {
            List<User> users = await _userManager.Users.ToListAsync();

            foreach (User user in users)
            {
                Faker<UserFollower> userFollowerFaker = new Faker<UserFollower>()
                  .RuleFor(x => x.FollowerId, _ => user.Id)
                  .RuleFor(x => x.Followed, f => f.PickRandom<User>(users.Where(u => u.Id != user.Id)));

                var userFollowers = userFollowerFaker.Generate(5)
                    .DistinctBy(x => x.Followed.Id)
                    .ToList();

                user.NumberOfFolloweds += userFollowers.Count;
                await _userManager.UpdateAsync(user);

                foreach (UserFollower userFollower in userFollowers)
                {
                    User followed = userFollower.Followed;
                    followed.NumberOfFollowers++;
                    await _userManager.UpdateAsync(followed);

                    userFollower.FollowedId = followed.Id;
                    await _context.UserFollowers.AddAsync(userFollower);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedFavouriteEvents()
    {
        if (!_context.FavouriteEvents.Any())
        {
            List<User> users = await _userManager.Users.ToListAsync();
            List<Domain.Aggregates.EventAggregate.Event> events = await _context.Events.AsNoTracking().ToListAsync();

            foreach (User user in users)
            {
                var favouriteEvents = new Faker().PickRandom<Domain.Aggregates.EventAggregate.Event>(events, 5)
                    .Select(x => new FavouriteEvent { EventId = x.Id, UserId = user.Id, Event = x })
                    .Distinct()
                    .ToList();

                user.NumberOfFavourites += favouriteEvents.Count;
                await _userManager.UpdateAsync(user);

                foreach (FavouriteEvent favouriteEvent in favouriteEvents)
                {
                    Domain.Aggregates.EventAggregate.Event @event = favouriteEvent.Event;
                    @event.NumberOfFavourites++;
                    _context.Events.Update(@event);

                    favouriteEvent.EventId = @event.Id;
                    await _context.FavouriteEvents.AddAsync(favouriteEvent);
                }

                await _context.FavouriteEvents.AddRangeAsync(favouriteEvents);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedReviews()
    {
        if (!_context.Reviews.Any())
        {
            List<User> users = await _userManager.Users.ToListAsync();
            List<Domain.Aggregates.EventAggregate.Event> events = await _context.Events.ToListAsync();

            Faker<Domain.Aggregates.EventAggregate.Entities.Review> fakerReview = new Faker<Domain.Aggregates.EventAggregate.Entities.Review>()
                .RuleFor(e => e.Content, f => f.Lorem.Text())
                .RuleFor(e => e.Rate, f => f.Random.Double(0.0, 5.0))
                .RuleFor(e => e.EventId, f => f.PickRandom<Domain.Aggregates.EventAggregate.Event>(events).Id)
                .RuleFor(e => e.IsPositive, f => f.Random.Bool())
                .RuleFor(e => e.AuthorId, f => f.PickRandom<User>(users).Id);

            List<Domain.Aggregates.EventAggregate.Entities.Review> reviews = fakerReview.Generate(1000);
            _context.Reviews.AddRange(reviews);

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedExpenses()
    {
        if (!_context.Expenses.Any())
        {
            var events = _context.Events.ToList();

            foreach (Domain.Aggregates.EventAggregate.Event @event in events)
            {
                Faker<Expense> expenseFaker = new Faker<Expense>()
                    .RuleFor(x => x.EventId, _ => @event.Id)
                    .RuleFor(x => x.Title, f => f.Commerce.Department());

                List<Expense> expenses = expenseFaker.Generate(3);
                await _context.Expenses.AddRangeAsync(expenses);
                await _context.SaveChangesAsync();

                foreach (Expense expense in expenses)
                {
                    Faker<SubExpense> subExpenseFaker = new Faker<SubExpense>()
                        .RuleFor(x => x.ExpenseId, _ => expense.Id)
                        .RuleFor(x => x.Name, f => f.Commerce.ProductMaterial())
                        .RuleFor(x => x.Price, f => f.Random.Long(100000, 10000000));

                    List<SubExpense> subExpenses = subExpenseFaker.Generate(3);
                    await _context.SubExpenses.AddRangeAsync(subExpenses);
                    await _context.SaveChangesAsync();

                    expense.Total = subExpenses.Sum(x => x.Price);
                    _context.Expenses.Update(expense);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
