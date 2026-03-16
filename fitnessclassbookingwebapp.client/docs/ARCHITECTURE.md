# Frontend Component Architecture

## Component Hierarchy

```
App (Root)
│
├── NavbarComponent (Always visible)
│   ├── Logo
│   ├── Navigation Links
│   ├── User Menu (if authenticated)
│   └── Auth Links (if not authenticated)
│
└── Router Outlet
    │
    ├── HomeComponent (/)
    │   ├── Hero Section
    │   ├── Featured Groups
    │   └── Upcoming Schedules
    │
    ├── LoginComponent (/login)
    │   └── Login Form
    │
    ├── RegisterComponent (/register)
    │   └── Registration Form
    │
    ├── GroupListComponent (/groups)
    │   ├── Search Box
    │   └── Groups Grid
    │
    ├── GroupDetailsComponent (/groups/:id)
    │   ├── Group Information
    │   ├── Upcoming Schedules List
    │   │   └── Book Button (per schedule)
    │   └── Reviews List
    │
    ├── BookingListComponent (/bookings) [Auth Required]
    │   └── Bookings List
    │       └── Cancel Button (per booking)
    │
    └── AdminDashboardComponent (/admin) [Admin Required]
        ├── Statistics Cards
        └── Management Links
```

## Data Flow Diagram

```
┌─────────────┐
│  Component  │
└──────┬──────┘
       │
       │ calls
       ▼
┌─────────────┐
│   Service   │
└──────┬──────┘
       │
       │ HTTP Request (via Interceptor)
       ▼
┌─────────────┐
│ Auth Inter- │
│   ceptor    │──► Adds JWT Token
└──────┬──────┘
       │
       │ Request with Token
       ▼
┌─────────────┐
│  Backend    │
│     API     │
└──────┬──────┘
       │
       │ Response
       ▼
┌─────────────┐
│  Component  │──► Update UI
└─────────────┘
```

## Service Layer Architecture

```
┌──────────────────────────────────────────┐
│           Component Layer                 │
│  (UI Logic, Template Binding)             │
└────────────┬─────────────────────────────┘
             │
             │ Inject Service
             ▼
┌──────────────────────────────────────────┐
│           Service Layer                   │
│  ┌────────────┐  ┌────────────┐          │
│  │ AuthService│  │GroupService│          │
│  └────────────┘  └────────────┘          │
│  ┌────────────┐  ┌────────────┐          │
│  │BookService │  │ReviewServic│          │
│  └────────────┘  └────────────┘          │
└────────────┬─────────────────────────────┘
             │
             │ HTTP Calls
             ▼
┌──────────────────────────────────────────┐
│         HTTP Client + Interceptor         │
└────────────┬─────────────────────────────┘
             │
             ▼
         Backend API
```

## State Management Flow

```
┌─────────────────┐
│  AuthService    │
│  (currentUser$) │◄──── BehaviorSubject stores auth state
└────────┬────────┘
         │
         │ Observables
         ▼
┌─────────────────┐
│   Components    │◄──── Subscribe to auth state
│   (Login UI)    │
└─────────────────┘

Login Flow:
1. User submits credentials
2. AuthService.login() called
3. HTTP POST to /api/auth/login
4. Token received
5. Store in localStorage
6. Update BehaviorSubject
7. All subscribed components notified
8. UI updates (show user menu)
```

## Route Guard Flow

```
User navigates to protected route
         │
         ▼
┌─────────────────┐
│  Route Guard    │
│  (canActivate)  │
└────────┬────────┘
         │
         ├──► Check: Is Authenticated?
         │         │
         │         ├── YES ──► Allow navigation
         │         │
         │         └── NO  ──► Redirect to /login
         │
         └──► Check: Is Admin? (for admin routes)
                   │
                   ├── YES ──► Allow navigation
                   │
                   └── NO  ──► Redirect to /
```

## Key Patterns Used

### 1. Observable Pattern
```typescript
// Service
currentUser$ = new BehaviorSubject<User | null>(null);

// Component
this.authService.currentUser$.subscribe(user => {
  this.currentUser = user;
});
```

### 2. Dependency Injection
```typescript
constructor(
  private authService: AuthService,
  private router: Router
) {}
```

### 3. Reactive Forms
```typescript
this.loginForm = this.fb.group({
  email: ['', [Validators.required, Validators.email]],
  password: ['', Validators.required]
});
```

### 4. HTTP Interceptor
```typescript
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = getToken();
  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }
  return next(req);
};
```

### 5. Route Guards
```typescript
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  if (authService.isAuthenticated()) {
    return true;
  }
  return router.navigate(['/login']);
};
```

## Component Communication

### Parent to Child
Not heavily used (mostly flat structure)

### Child to Parent
Event emitters (if needed)

### Sibling Components
Via shared service (AuthService)

### Route Parameters
```typescript
// In routing
{ path: 'groups/:id', component: GroupDetailsComponent }

// In component
const id = this.route.snapshot.paramMap.get('id');
```

### Query Parameters
```typescript
// In guard
router.navigate(['/login'], { 
  queryParams: { returnUrl: state.url } 
});

// In component
this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
```

## Styling Architecture

```
Global Styles (styles.css)
├── Reset & Base styles
├── Typography
└── Utility classes

Component Styles (*.component.css)
├── Scoped to component
├── BEM-like naming
└── Responsive breakpoints

Theme
├── Primary: #667eea (Purple-Blue)
├── Secondary: #764ba2 (Purple)
├── Background: #f5f7fa
└── Shadows: rgba(0,0,0,0.08-0.15)
```

## Error Handling

```
HTTP Error
    │
    ▼
Service catches error
    │
    ▼
Returns error to component
    │
    ▼
Component displays error
    │
    ├──► errorMessage variable
    └──► Alert/Toast notification
```

## Best Practices Implemented

✅ **Services for business logic**  
✅ **Components for UI only**  
✅ **Models for type safety**  
✅ **Guards for route protection**  
✅ **Interceptors for cross-cutting concerns**  
✅ **Reactive forms for validation**  
✅ **Observable pattern for async**  
✅ **Lazy loading ready structure**  
✅ **Mobile-responsive design**  
✅ **Semantic HTML**  

## File Naming Conventions

- **Components**: `feature-name.component.ts/html/css`
- **Services**: `feature-name.service.ts`
- **Models**: `feature-name.model.ts`
- **Guards**: `guard-name.guard.ts`
- **Interceptors**: `interceptor-name.interceptor.ts`

## Module Organization

```
AppModule (Root Module)
├── Declarations: All Components
├── Imports: 
│   ├── BrowserModule
│   ├── HttpClientModule
│   ├── FormsModule
│   ├── ReactiveFormsModule
│   └── AppRoutingModule
└── Providers:
    └── HTTP Interceptor
```

---

**This architecture provides:**
- Clear separation of concerns
- Maintainable code structure
- Scalable design
- Type safety
- Reusable components
- Consistent patterns
