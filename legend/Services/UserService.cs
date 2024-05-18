namespace legend.Services;

using AutoMapper;
using BCrypt.Net;
using legend.Authorization;
using legend.Entities;
using legend.Helpers;
using legend.Models.Users;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    User GetById(Guid id);
    void Register(RegisterRequest model);
    void Update(Guid id, UpdateRequest model);
    void Delete(Guid id);
}

public class UserService : IUserService
{
    private DataContext _context;
    private IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;

    public UserService(
        DataContext context,
        IJwtUtils jwtUtils,
        IMapper mapper)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        // authentication successful
        var response = _mapper.Map<AuthenticateResponse>(user);
        response.Token = _jwtUtils.GenerateToken(user);
        return response;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User GetById(Guid id)
    {
        return getUser(id);
    }

    public void Register(RegisterRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.Username == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.PasswordHash = BCrypt.HashPassword(model.Password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(Guid id, UpdateRequest model)
    {
        var user = getUser(id);

        // validate
        if (model.Username != user.Username && _context.Users.Any(x => x.Username == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);

        // copy model to user and save
        _mapper.Map(model, user);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var user = getUser(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    // helper methods

    private User getUser(Guid id)
    {
        var user = _context.Users
            .Include(user => user.DeliveryAddresses)
            .FirstOrDefault(u => u.Id == id);

        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
}