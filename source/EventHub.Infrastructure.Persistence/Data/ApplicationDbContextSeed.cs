﻿using Bogus;
using EventHub.Domain.Aggregates.CategoryAggregate;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.Aggregates.ReviewAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Shared.Enums.Event;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

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
        await SeedPaymentMethods();
        await SeedEvents();
        await SeedReviews();
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
                },
                new()
                {
                    Id = EFunctionCode.SYSTEM_COMMAND.GetDisplayName(), Name = "Commands",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/command"
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
                new() { Id = "APPROVE", Name = "Approve" }
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
                    FunctionId = EFunctionCode.SYSTEM.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.SYSTEM.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "UPDATE"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.SYSTEM_USER.GetDisplayName(),
                    RoleId = customerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.SYSTEM_USER.GetDisplayName(),
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
                    FunctionId = EFunctionCode.SYSTEM_USER.GetDisplayName(),
                    RoleId = organizerRole.Id,
                    CommandId = "VIEW"
                });
                _context.Permissions.Add(new Permission
                {
                    FunctionId = EFunctionCode.SYSTEM_USER.GetDisplayName(),
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

            for (int i = 0; i < bankNames.Count; i++)
            {
                var method = new PaymentMethod
                {
                    MethodLogoFileName = icons[i],
                    MethodName = bankNames[i],
                    MethodLogoUrl = string.Empty,
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
            List<User> users = await _userManager.Users.ToListAsync();
            List<Category> categories = await _context.Categories.ToListAsync();

            Faker<Event> fakerEvent = new Faker<Event>()
                .RuleFor(e => e.AuthorId, f => f.PickRandom<User>(users).Id)
                .RuleFor(e => e.Name, f => f.Commerce.ProductName())
                .RuleFor(e => e.Description, f => f.Commerce.ProductDescription())
                .RuleFor(e => e.Promotion, f => f.Random.Double())
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
                Event eventItem = fakerEvent.Generate();
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
                IEnumerable<EventCategory> eventCategories = fakerEventCategory.GenerateBetween(1, 3).DistinctBy(ec => ec.CategoryId);
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
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedReviews()
    {
        if (!_context.Reviews.Any())
        {
            List<User> users = await _userManager.Users.ToListAsync();
            List<Event> events = await _context.Events.ToListAsync();

            Faker<Review> fakerReview = new Faker<Review>()
                .RuleFor(e => e.Content, f => f.Lorem.Text())
                .RuleFor(e => e.Rate, f => f.Random.Double(0.0, 5.0))
                .RuleFor(e => e.EventId, f => f.PickRandom<Event>(events).Id)
                .RuleFor(e => e.AuthorId, f => f.PickRandom<User>(users).Id);

            List<Review> reviews = fakerReview.Generate(1000);
            _context.Reviews.AddRange(reviews);

            await _context.SaveChangesAsync();
        }
    }
}
