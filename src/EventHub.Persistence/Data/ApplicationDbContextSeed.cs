using Bogus;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Shared.Enums.Event;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;

namespace EventHub.Persistence.Data;

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
        SeedRoles().Wait();
        SeedUsers().Wait();
        SeedFunctions().Wait();
        SeedCommands().Wait();
        SeedPermission().Wait();
        SeedCategories().Wait();
        SeedPaymentMethods().Wait();
        SeedEvents().Wait();
        SeedReviews().Wait();
    }

    private async Task SeedRoles()
    {
        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new Role
            {
                Name = EUserRole.ADMIN.GetDisplayName(),
                ConcurrencyStamp = "1",
                NormalizedName = EUserRole.ADMIN.GetDisplayName().Normalize()
            });
            await _roleManager.CreateAsync(new Role
            {
                Name = EUserRole.CUSTOMER.GetDisplayName(),
                ConcurrencyStamp = "2",
                NormalizedName = EUserRole.CUSTOMER.GetDisplayName().Normalize()
            });
            await _roleManager.CreateAsync(new Role
            {
                Name = EUserRole.ORGANIZER.GetDisplayName(),
                ConcurrencyStamp = "3",
                NormalizedName = EUserRole.ORGANIZER.GetDisplayName().Normalize()
            });
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
                NormalizedEmail = "admin@gmail.com",
                UserName = "admin",
                NormalizedUserName = "admin",
                LockoutEnabled = false,
                PhoneNumber = "0829440357",
                FullName = "Admin",
                Dob = new Faker().Person.DateOfBirth,
                Gender = EGender.MALE,
                Bio = new Faker().Lorem.ToString(),
                Status = EUserStatus.ACTIVE
            };
            var result = await _userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync("admin@gmail.com");
                await _userManager.AddToRoleAsync(user, EUserRole.ADMIN.GetDisplayName());
            }


            var userFaker = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.NormalizedEmail, f => f.Person.Email)
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.NormalizedUserName, f => f.Person.UserName)
                .RuleFor(u => u.LockoutEnabled, f => false)
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("###-###-####"))
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Dob, f => f.Person.DateOfBirth)
                .RuleFor(u => u.Gender, f => f.PickRandom<EGender>())
                .RuleFor(u => u.Bio, f => f.Lorem.ToString())
                .RuleFor(u => u.Status, _ => EUserStatus.ACTIVE);

            for (var userIndex = 0; userIndex < MAX_USERS_QUANTITY * 2; userIndex++)
            {
                var customer = userFaker.Generate();
                var customerResult = await _userManager.CreateAsync(customer, "User@123");
                if (customerResult.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(customer.Email);
                    await _userManager.AddToRolesAsync(user,
                        new List<string> { EUserRole.CUSTOMER.GetDisplayName(), EUserRole.ORGANIZER.GetDisplayName() });
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
                    Id = EFunctionCode.DASHBOARD.GetDisplayName(), Name = "Dashboard", ParentId = null, SortOrder = 0,
                    Url = "/dashboard"
                },

                new()
                {
                    Id = EFunctionCode.GENERAL.GetDisplayName(), Name = "General", ParentId = null, SortOrder = 0,
                    Url = "/general"
                },

                new()
                {
                    Id = EFunctionCode.GENERAL_CATEGORY.GetDisplayName(), Name = "Categories",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/general/category"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_EVENT.GetDisplayName(), Name = "Events",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 1, Url = "/general/event"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_REVIEW.GetDisplayName(), Name = "Reviews",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 2, Url = "/general/review"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_TICKET.GetDisplayName(), Name = "Tickets",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 2, Url = "/general/ticket"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_CHAT.GetDisplayName(), Name = "Chats",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 2, Url = "/general/chat"
                },
                new()
                {
                    Id = EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), Name = "Payments",
                    ParentId = EFunctionCode.GENERAL.GetDisplayName(), SortOrder = 2, Url = "/general/payment"
                },

                new()
                {
                    Id = EFunctionCode.STATISTIC.GetDisplayName(), Name = "Statistics", ParentId = null, SortOrder = 0,
                    Url = "/statistic"
                },

                new()
                {
                    Id = EFunctionCode.SYSTEM.GetDisplayName(), Name = "System", ParentId = null, SortOrder = 0,
                    Url = "/system"
                },

                new()
                {
                    Id = EFunctionCode.SYSTEM_USER.GetDisplayName(), Name = "Users",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/user"
                },
                new()
                {
                    Id = EFunctionCode.SYSTEM_ROLE.GetDisplayName(), Name = "Roles",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/role"
                },
                new()
                {
                    Id = EFunctionCode.SYSTEM_FUNCTION.GetDisplayName(), Name = "Functions",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/function"
                },
                new()
                {
                    Id = EFunctionCode.SYSTEM_PERMISSION.GetDisplayName(), Name = "Permissions",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/permission"
                }
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
                new() { Id = "APPROVE", Name = "Approve" }
            });

            await _context.SaveChangesAsync();
        }

        if (!_context.CommandInFunctions.Any())
        {
            var functions = _context.Functions;

            foreach (var function in functions)
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
            var functions = _context.Functions;
            var adminRole = await _roleManager.FindByNameAsync(EUserRole.ADMIN.GetDisplayName());
            foreach (var function in functions)
            {
                _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "CREATE"));
                _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "UPDATE"));
                _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "DELETE"));
                _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "VIEW"));
            }

            var customerRole = await _roleManager.FindByNameAsync(EUserRole.CUSTOMER.GetDisplayName());
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL.GetDisplayName(), customerRole.Id, "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL.GetDisplayName(), customerRole.Id, "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL.GetDisplayName(), customerRole.Id, "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL.GetDisplayName(), customerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CATEGORY.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_EVENT.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), customerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), customerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), customerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_TICKET.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_REVIEW.GetDisplayName(), customerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_REVIEW.GetDisplayName(), customerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_REVIEW.GetDisplayName(), customerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_REVIEW.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(
                new Permission(EFunctionCode.GENERAL_CHAT.GetDisplayName(), customerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CHAT.GetDisplayName(), customerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CHAT.GetDisplayName(), customerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CHAT.GetDisplayName(), customerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM.GetDisplayName(), customerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM.GetDisplayName(), customerRole.Id, "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM_USER.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM_USER.GetDisplayName(), customerRole.Id,
                "UPDATE"));

            var organizerRole = await _roleManager.FindByNameAsync(EUserRole.ORGANIZER.GetDisplayName());
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL.GetDisplayName(), organizerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CATEGORY.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_EVENT.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_EVENT.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_EVENT.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_EVENT.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_PAYMENT.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_TICKET.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_TICKET.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_TICKET.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_TICKET.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_REVIEW.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_REVIEW.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CHAT.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CHAT.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CHAT.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.GENERAL_CHAT.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(
                new Permission(EFunctionCode.SYSTEM_USER.GetDisplayName(), organizerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM_USER.GetDisplayName(), organizerRole.Id,
                "UPDATE"));

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

            var fakerCategory = new Faker<Category>()
                .RuleFor(c => c.Color, f => f.Commerce.Color());

            var categories = new List<Category>();
            for (var i = 0; i < MAX_CATEGORIES_QUANTITY; i++)
            {
                fakerCategory
                    .RuleFor(c => c.Name, _ => categoryNames[i])
                    .RuleFor(c => c.IconImage, _ => icons[i]);

                var category = fakerCategory.Generate();
                categories.Add(category);
            }

            _context.Categories.AddRange(categories);

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedPaymentMethods()
    {
        if (!_context.PaymentMethods.Any())
        {
            #region Categories Data

            // WARNING: Must not change the items' order, just add more!
            var bankNames = new List<string>
            {
                "ACB", "Agribank", "BIDV", "HDBank", "MBBank", "Momo", "MSB", "OceanBank", "Sacombank", "SCB",
                "SeABank", "SHB", "Techcombank", "TPBank", "Vietcombank", "Vietinbank", "VPBank", "ZaloPay"
            };

            var icons = new List<string>
            {
                "acb.png",
                "agribank.png",
                "bidv.png",
                "hdbank.png",
                "mbb.png",
                "momo.png",
                "msb.png",
                "oceanbank.png",
                "sacombank.png",
                "scb.png",
                "seabank.png",
                "shb.png",
                "tcb.png",
                "tpbank.png",
                "vietcombank.png",
                "vietinbank.png",
                "vpbank.png",
                "zalopay.png"
            };

            #endregion

            var paymentMethods = new List<PaymentMethod>();
            for (var i = 0; i < bankNames.Count; i++)
            {
                var method = new PaymentMethod
                {
                    MethodLogo = icons[i],
                    MethodName = bankNames[i]
                };
                _context.PaymentMethods.Add(method);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedEvents()
    {
        if (!_context.Events.Any())
        {
            var users = _userManager.Users;
            var categories = _context.Categories;

            var fakerEvent = new Faker<Event>()
                .RuleFor(e => e.AuthorId, f => f.PickRandom<User>(users).Id)
                .RuleFor(e => e.Name, f => f.Commerce.ProductName())
                .RuleFor(e => e.Description, f => f.Commerce.ProductDescription())
                .RuleFor(e => e.Promotion, f => f.Random.Double())
                .RuleFor(e => e.Location, f => f.Address.FullAddress())
                .RuleFor(e => e.EventPaymentType, f => f.Random.Enum<EEventPaymentType>())
                .RuleFor(e => e.EventCycleType, f => f.Random.Enum<EEventCycleType>());

            var fakerTicketType = new Faker<TicketType>()
                .RuleFor(t => t.Name, f => f.Commerce.ProductMaterial())
                .RuleFor(t => t.Quantity, f => f.Random.Int(0, 1000))
                .RuleFor(t => t.Price, f => f.Random.Long(0, 100000000));

            var fakerEmailContent = new Faker<EmailContent>()
                .RuleFor(e => e.Content,
                    _ =>
                        "<p>Dear attendee,<br>Thank you for attending our event! We hope you found it informative and enjoyable<br>Best regards,<br>EventHub</p>");

            var fakerReason = new Faker<Reason>()
                .RuleFor(t => t.Name, f => f.Commerce.ProductDescription());

            var fakerEventCategory = new Faker<EventCategory>()
                .RuleFor(ec => ec.CategoryId, f => f.PickRandom<Category>(categories).Id);

            for (var eventIndex = 0; eventIndex < MAX_EVENTS_QUANTITY; eventIndex++)
            {
                #region EventTime

                var eventStartTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(new Faker().Random.Number(0, 60)));
                var eventEndTime = DateTime.UtcNow.Add(TimeSpan.FromDays(new Faker().Random.Number(1, 60)));
                var eventItem = fakerEvent.Generate();
                eventItem.StartTime = eventStartTime;
                eventItem.EndTime = eventEndTime;
                if (eventItem.StartTime <= DateTime.UtcNow && DateTime.UtcNow <= eventItem.EndTime)
                    eventItem.Status = EEventStatus.OPENING;
                else if (DateTime.UtcNow < eventItem.StartTime)
                    eventItem.Status = EEventStatus.UPCOMING;
                else
                    eventItem.Status = EEventStatus.CLOSED;

                #endregion

                #region CoverImage

                eventItem.CoverImage = new Faker().Image.ToString();

                #endregion

                _context.Events.Add(eventItem);

                #region EmailContent

                fakerEmailContent.RuleFor(ec => ec.EventId, _ => eventItem.Id);
                var emailContent = fakerEmailContent.Generate();
                _context.EmailContents.Add(emailContent);

                #endregion

                #region EventCategories

                fakerEventCategory.RuleFor(ec => ec.EventId, _ => eventItem.Id);
                var eventCategories = fakerEventCategory.GenerateBetween(1, 3).DistinctBy(ec => ec.CategoryId);
                _context.EventCategories.AddRange(eventCategories);

                #endregion

                #region EventSubImages

                var subImages = new Faker<EventSubImage>().Generate(5);
                subImages.ForEach(image =>
                {
                    var eventSubImage = new EventSubImage
                    {
                        EventId = eventItem.Id,
                        Image = new Faker().Image.ToString()
                    };
                    _context.EventSubImages.Add(eventSubImage);
                });

                #endregion

                #region TicketTypes

                fakerTicketType.RuleFor(t => t.EventId, _ => eventItem.Id);
                var ticketTypes = fakerTicketType.Generate(3);
                _context.TicketTypes.AddRange(ticketTypes);

                #endregion

                #region Reasons

                fakerReason.RuleFor(t => t.EventId, _ => eventItem.Id);
                var reasons = fakerReason.Generate(3);
                _context.Reasons.AddRange(reasons);

                #endregion
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedReviews()
    {
        if (!_context.Reviews.Any())
        {
            var users = _userManager.Users;
            var events = _context.Events;

            var fakerReview = new Faker<Review>()
                .RuleFor(e => e.Content, f => f.Lorem.Text())
                .RuleFor(e => e.Rate, f => f.Random.Double(0.0, 5.0))
                .RuleFor(e => e.EventId, f => f.PickRandom<Event>(events).Id)
                .RuleFor(e => e.UserId, f => f.PickRandom<User>(users).Id);

            var reviews = fakerReview.Generate(1000);
            _context.Reviews.AddRange(reviews);

            await _context.SaveChangesAsync();
        }
    }
}