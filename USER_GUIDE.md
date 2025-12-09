# NatureHelp User Guide

## Table of Contents
1. [About NatureHelp](#about-naturehelp)
2. [Getting Started](#getting-started)
3. [User Roles & Permissions](#user-roles--permissions)
4. [Authentication & Email Confirmation](#authentication--email-confirmation)
5. [Navigating the Application](#navigating-the-application)
6. [Environmental Monitoring](#environmental-monitoring)
7. [Laboratory Management](#laboratory-management)
8. [Organization Management](#organization-management)
9. [Reports & Analytics](#reports--analytics)
10. [Data Privacy & Public Access](#data-privacy--public-access)

## About NatureHelp

NatureHelp is an environmental monitoring and reporting platform designed to track, analyze, and manage environmental data. The system enables organizations to:

- **Monitor Environmental Quality**: Track water and soil contamination levels
- **Record Research Data**: Document laboratory analyses and field studies
- **Manage Organizations**: Coordinate teams and assign responsibilities
- **Generate Reports**: Export data for analysis and compliance
- **Visualize Data**: View environmental issues on interactive maps
- **Track Changes**: Maintain complete audit trails of all data modifications

## Getting Started

### Accessing the Application

**Live Application**: https://naturehelp.online/

### System Requirements

- Modern web browser (Chrome, Firefox, Safari, Edge)
- Stable internet connection
- Email account for registration

## User Roles & Permissions

NatureHelp uses a role-based access control system with five distinct roles:

### Role Hierarchy

#### 1. Super Admin (Level 0)
**Purpose**: System-wide administration

**Permissions**:
- Full access to all system features
- Manage all organizations
- Override any restrictions
- System configuration access

**Use Case**: Platform administrators only

#### 2. Owner (Level 1)
**Purpose**: Organization leadership and management

**Permissions**:
- Create and manage organization
- Add/remove users to organization
- Assign roles to organization members (Manager, Supervisor, Researcher)
- Change user roles within organization
- View all organization data
- Create, edit, and delete deficiency reports
- Access all reports and analytics
- Manage laboratories within organization

**Use Case**: Organization directors, heads of environmental departments

#### 3. Manager (Level 2)
**Purpose**: Day-to-day operations management

**Permissions**:
- View all organization data
- Create, edit, and delete deficiency reports
- Assign responsibilities to Supervisors and Researchers
- Access reports and analytics
- Manage monitoring schemes
- Cannot add/remove users or change roles

**Use Case**: Project managers, senior environmental specialists

#### 4. Supervisor (Level 3)
**Purpose**: Field work and data collection

**Permissions**:
- View organization data
- Create, edit, and delete deficiency reports
- Update assigned deficiencies
- Add comments and attachments
- View reports (cannot generate)
- Default role for new users

**Use Case**: Field inspectors, environmental technicians

#### 5. Researcher (Level 4)
**Purpose**: Read-only access for research and analysis

**Permissions**:
- View public deficiency data
- View laboratory research data
- View organization information
- Cannot create or modify any data
- Limited report access

**Use Case**: Academic researchers, data analysts, interns

### Role Assignment

**Only Owners** can assign or change user roles within their organization. When an Owner adds a new user:

1. Owner creates user account with initial role
2. User receives invitation email
3. User sets password on first login
4. Role can be changed later by Owner through user management

## Authentication & Email Confirmation

### Registration Process

#### Step 1: User Creation
New users are created by Organization Owners:

1. Owner navigates to **Organization Management** → **Users**
2. Clicks **Add New User**
3. Fills in user details:
   - First Name
   - Last Name
   - Email Address
   - Initial Role (Manager/Supervisor/Researcher)
4. Submits form

#### Step 2: Email Confirmation
After account creation:

1. System sends confirmation email to user's address
2. Email contains unique confirmation link
3. User clicks link to verify email
4. Confirmation link format: `https://https://naturehelp.online//confirm-email?token=[unique-token]`
5. Upon confirmation, user can proceed to set password

**Note**: Email confirmation is required before first login.

#### Step 3: First Login
Once email is confirmed:

1. User navigates to application homepage
2. Clicks **Login** button
3. Enters email and password
4. System generates:
   - **Access Token**: Valid for 10 minutes (for API requests)
   - **Refresh Token**: Valid for 15 days (for session renewal)
5. User is redirected to dashboard

### Password Requirements

Passwords must meet these security criteria:

- **Minimum 8 characters**
- At least 1 uppercase letter (A-Z)
- At least 1 lowercase letter (a-z)
- At least 1 digit (0-9)
- At least 1 special character (@$!%*?&)

**Example Valid Passwords**:
- `SecurePass123!`
- `EnviroData@2025`
- `Water$Quality9`

### Password Reset

If you forget your password:

1. Click **Forgot Password** on login page
2. Enter your email address
3. Receive password reset email
4. Click reset link (valid for limited time)
5. Enter new password
6. Login with new credentials

### Email Confirmation Issues

**Email not received?**
- Check spam/junk folder
- Verify email address is correct
- Request resend from login page
- Contact your organization Owner

**Confirmation link expired?**
- Contact your organization Owner
- Owner can regenerate confirmation email

### Session Management

**Access Token**: 
- Expires after 10 minutes of inactivity
- Automatically refreshed on user activity
- Used for authenticating API requests

**Refresh Token**:
- Expires after 15 days
- Used to obtain new access token
- Requires re-login after expiration

**Auto-Login**:
- Application remembers authenticated users
- Uses stored refresh token
- Auto-reconnects if token is valid

## Navigating the Application

### Main Navigation Menu

The primary navigation bar provides access to all major features:

#### 1. **Water Deficiencies** (Default Home)
Path: `/water` or `/uk/water`

View all reported water quality issues on interactive map and table.

#### 2. **Soil Deficiencies**
Path: `/soil` or `/uk/soil`

Access soil contamination data and reports.

#### 3. **Laboratories**
Path: `/laboratories`

Manage laboratory information and research data.

#### 4. **Research**
Path: `/research`

View all research records and studies.

#### 5. **Organizations**
Path: `/organizations`

Browse registered organizations (public directory).

#### 6. **My Organization**
Path: `/owner/organization` (Owners only)

Manage your organization's users and settings.

#### 7. **Analytics**
Path: `/analytics` (Owners/Managers/Supervisors only)

Access reports and export functionality.

#### 8. **Profile**
Access user account settings and preferences.

### Language Support

NatureHelp supports multiple languages:

- **Ukrainian** (default): `/uk/[page]`
- **English**: `/en/[page]`

Change language using language selector in navigation bar.

## Environmental Monitoring

### Water Deficiency Monitoring

Water deficiencies track contamination and quality issues in water bodies.

#### Viewing Water Deficiencies

**Map View**:
1. Navigate to **Water** section
2. Interactive map displays all deficiency locations
3. Markers color-coded by danger level:
   - **Green**: Moderate (safe levels)
   - **Yellow**: Dangerous (concerning levels)
   - **Red**: Critical (immediate action needed)
4. Click marker to view details
5. Radius circle shows affected area

**Table View**:
1. Scroll below map for tabular data
2. Columns display:
   - Title
   - Location (coordinates/address)
   - Danger State
   - Responsible User
   - Created Date
   - Public/Private status
3. Click row to view full details
4. Use infinite scroll to load more entries

#### Creating Water Deficiency Report

**Required Role**: Supervisor or higher

**Steps**:
1. Navigate to **Water** → **Add New**
2. Fill required fields:

**Basic Information**:
- **Title**: Descriptive name (e.g., "River Dnipro Contamination - Kyiv")
- **Description**: Detailed observations and context
- **Location**: Click map or enter coordinates
  - Latitude (decimal degrees)
  - Longitude (decimal degrees)
  - Address (optional, for reference)
- **Radius Affected**: Area impacted in meters
- **Danger State**: Select level (Moderate/Dangerous/Critical)
- **Responsible User**: Assign to team member
- **Public**: Toggle visibility (see [Data Privacy](#data-privacy--public-access))

**Chemical Parameters**:
- **pH Level** (0-14): Acidity/alkalinity
- **Dissolved Oxygen** (0-20 mg/L): Oxygen content
- **Lead Concentration** (0-0.01 mg/L): Heavy metal contamination
- **Mercury Concentration** (0-0.001 mg/L): Toxic metal levels
- **Nitrate Concentration** (0-50 mg/L): Nitrogen compounds
- **Pesticides Content** (0-0.005 mg/L): Agricultural chemicals
- **Cadmium Concentration** (mg/L): Heavy metal
- **Phosphate Concentration** (mg/L): Nutrient levels

**Biological & Physical Parameters**:
- **Microbial Activity** (0-1000 CFU/mL): Bacterial presence
- **Microbial Load** (CFU/mL): Total microorganisms
- **Radiation Level** (0-10 μSv/h): Radioactivity
- **Chemical Oxygen Demand (COD)** (0-1000 mg/L): Organic pollution
- **Biological Oxygen Demand (BOD)** (0-500 mg/L): Biodegradable matter
- **Total Dissolved Solids (TDS)** (mg/L): Mineral content
- **Electrical Conductivity** (μS/cm): Ion concentration

3. **Attach Files** (optional):
   - Photos of site
   - Lab results
   - Supporting documents
   - Maximum 10 files per report

4. Click **Submit**

**Validation**: All parameters are validated against safe ranges. Out-of-range values may trigger warnings.

#### Editing Water Deficiency

**Required Role**: Supervisor or higher (must be creator or assigned user)

1. Navigate to deficiency detail page
2. Click **Edit** button
3. Modify any fields
4. Changes are logged in audit trail
5. Click **Save Changes**

**Note**: Edit history is tracked. System records:
- What changed
- Who made the change
- When it was changed
- Previous value

#### Deleting Water Deficiency

**Required Role**: Manager or higher

1. Open deficiency detail page
2. Click **Delete** button
3. Confirm deletion
4. Soft deletion (data retained in audit log)

### Soil Deficiency Monitoring

Soil deficiencies track contamination and degradation in soil samples.

#### Viewing Soil Deficiencies

Same interface as water deficiencies:
- Map view with location markers
- Table view with sortable columns
- Color-coded danger levels

#### Creating Soil Deficiency Report

**Required Role**: Supervisor or higher

**Steps**:
1. Navigate to **Soil** → **Add New**
2. Fill required fields:

**Basic Information**:
- Title, Description, Location (same as water)
- Radius Affected
- Danger State
- Responsible User
- Public/Private flag

**Soil Chemical Parameters**:
- **pH Level**: Soil acidity (typical range 4-9)
- **Organic Matter** (%): Organic content
- **Lead Concentration** (mg/kg): Heavy metal in soil
- **Cadmium Concentration** (mg/kg): Toxic metal
- **Mercury Concentration** (mg/kg): Mercury contamination
- **Pesticides Content** (mg/kg): Chemical residues
- **Nitrates Concentration** (mg/kg): Nitrogen compounds
- **Heavy Metals Concentration** (mg/kg): Copper, zinc, etc.

**Soil Physical & Biological Parameters**:
- **Electrical Conductivity** (mS/cm): Salinity indicator
- **Microbial Activity** (CFU/g): Soil microorganisms
- **Erosion Risk** (1-10 scale): Degradation risk
- **Analysis Date**: When soil sample was collected

3. Attach supporting files
4. Click **Submit**

#### Danger State Assessment

System helps classify danger levels based on parameter thresholds:

**Moderate**: Values within acceptable ranges
- Minor deviations from standards
- No immediate health risk
- Monitoring recommended

**Dangerous**: Values exceed safe thresholds
- Potential health or environmental risk
- Action plan required
- Regular monitoring mandatory

**Critical**: Severe contamination detected
- Immediate health hazard
- Emergency response needed
- Area may need restriction

### Monitoring Schemes

**Purpose**: Track deficiencies over time with scheduled checks

**Required Role**: Manager or higher

**Features**:
- Assign monitoring schemes to users
- Set check-in intervals (daily, weekly, monthly)
- Track compliance with monitoring schedule
- Receive notifications for overdue checks

**Creating Monitoring Scheme**:
1. Open deficiency detail page
2. Click **Create Monitoring Scheme**
3. Configure:
   - Check-in frequency
   - Assigned user
   - Parameters to monitor
   - Alert thresholds
4. Save scheme

**User Dashboard**:
- Assigned monitoring tasks appear in user dashboard
- Mark checks as complete
- Add observations for each check
- System tracks monitoring history

## Laboratory Management

### Viewing Laboratories

Path: `/laboratories`

**Features**:
- View all registered laboratories
- See laboratory locations on map
- Filter by research type
- View associated research

**Laboratory Details**:
- Laboratory name
- Address and coordinates
- Contact information
- Research types performed
- Associated organization
- Active research projects

### Research Records

Path: `/research`

**Purpose**: Document scientific studies and laboratory analyses

**Research Types**:

**Soil Research**:
1. **Soil Chemical Analysis**: Nutrient and contaminant testing
2. **Soil Profile Study**: Geological layer analysis
3. **Geochemical Research**: Mineral composition studies

**Water Research**:
4. **Water Physical-Chemical Analysis**: Quality parameter testing
5. **Water Biological Monitoring**: Ecosystem health assessment
6. **Hydromorphological Analysis**: Water body structure studies

### Viewing Research

**Table View** displays:
- Research title
- Research type
- Date conducted
- Researcher name
- Laboratory name and location

**Detail View** shows:
- Complete methodology
- Results and findings
- Associated deficiency reports
- Attachments and documentation

### Creating Research Records

**Required Role**: Researcher or higher

1. Navigate to **Research** → **Add New**
2. Fill details:
   - **Title**: Research project name
   - **Type**: Select from research types
   - **Date**: When conducted
   - **Laboratory**: Select associated lab
   - **Researcher**: Conducting scientist
   - **Description**: Methods and findings
   - **Related Deficiencies**: Link to reports
3. Attach lab results and documentation
4. Submit

## Organization Management

### Viewing Organizations

Path: `/organizations`

**Public Directory**: Browse all registered organizations

**Information Displayed**:
- Organization name
- Member count
- Location/service area
- Active deficiencies
- Public contact information

### Managing Your Organization

Path: `/owner/organization`

**Required Role**: Owner

#### Organization Settings

**Edit Organization**:
1. Navigate to **My Organization**
2. Click **Edit Settings**
3. Update:
   - Organization name
   - Allowed members count (subscription-based)
   - Contact information
   - Service area
4. Save changes

#### User Management

**Viewing Organization Users**:
1. Navigate to **My Organization** → **Users**
2. See complete member list
3. View for each user:
   - Name and email
   - Current role
   - Email confirmation status
   - Last login date
   - Assigned monitoring tasks

**Adding New User**:
1. Click **Add New User**
2. Enter user details:
   - First Name
   - Last Name
   - Email (must be unique)
   - Role (Manager/Supervisor/Researcher)
3. Submit
4. User receives invitation email
5. User appears in "Not Logged In" list until first login

**Changing User Roles**:
1. Select user from table
2. Click **Change Role**
3. Select new role from dropdown
4. Confirm change
5. User permissions update immediately

**Viewing Inactive Users**:
- Special view shows users who never logged in
- Useful for tracking pending invitations
- Can resend invitation emails

**Removing Users**:
1. Select user
2. Click **Remove from Organization**
3. Confirm removal
4. User loses access to organization data
5. User account remains (can be added to another organization)

#### Bulk User Operations

**Add Multiple Users**:
1. Click **Bulk Add**
2. Upload CSV file or enter list
3. Format: Email, FirstName, LastName, Role
4. System validates and creates accounts
5. Batch sends invitation emails

## Reports & Analytics

Path: `/analytics`

**Required Role**: Supervisor or higher (viewing), Manager or higher (generating)

### Available Reports

#### 1. Water Deficiencies Report
**Format**: Excel (.xlsx)

**Contents**:
- Complete water deficiency dataset
- All chemical parameters
- Location data
- Danger classifications
- Timestamps and responsible users

**Access**: 
- API: `GET /api/report/water`
- Downloads automatically as Excel file

#### 2. Soil Deficiencies Report
**Format**: Excel (.xlsx)

**Contents**:
- Complete soil deficiency dataset
- All soil parameters
- Contamination levels
- Geographic data
- Analysis dates

**Access**:
- API: `GET /api/report/soil`

#### 3. Organization Users Report
**Format**: Excel (.xlsx)

**Contents**:
- Complete user list
- Roles and permissions
- Contact information
- Activity status
- Last login dates

**Access**:
- API: `GET /api/report/org-users`
- **Required Role**: Owner only

### Generating Reports

**Via UI**:
1. Navigate to **Analytics**
2. Select report type
3. Choose date range (if applicable)
4. Click **Generate**
5. File downloads automatically

**Via API**:
- Use Swagger documentation at `/swagger`
- Authenticate with Bearer token
- Call report endpoint
- Receive file in response

### Data Filters

Reports can be filtered by:
- Date range
- Organization
- Danger state
- Geographic area
- Responsible user
- Public/private status

## Data Privacy & Public Access

### Public vs Private Deficiencies

NatureHelp supports both public and private deficiency reports:

#### Public Deficiencies
**Visibility**: Anyone can view, including anonymous users

**Use Cases**:
- Transparency in environmental issues
- Community awareness
- Public health information
- Research and education

**Displayed On**:
- Public map view
- Public API endpoints
- Search results
- External integrations

#### Private Deficiencies
**Visibility**: Only organization members can view

**Use Cases**:
- Internal investigations
- Sensitive locations
- Preliminary data before verification
- Confidential organizational matters

**Restrictions**:
- Not shown on public map
- Excluded from public API
- Only accessible with authentication
- Organization members only

### Setting Privacy

**When Creating**:
- Toggle **Public** checkbox
- Default: Public (for transparency)
- Can change after creation

**After Creation**:
- Edit deficiency
- Toggle public/private flag
- Change takes effect immediately

### Anonymous Access

**What Anonymous Users Can See**:
- Public water deficiencies
- Public soil deficiencies
- Public map with markers
- Organization directory
- Laboratory list
- Public research summaries

**What Requires Authentication**:
- Creating or editing any data
- Viewing private deficiencies
- Accessing organization management
- Generating reports
- Viewing user information
- Adding comments or attachments

## Domain Logic & Business Rules

### Data Validation

#### Location Data
- **Coordinates**: Must be valid decimal degrees
  - Latitude: -90 to +90
  - Longitude: -180 to +180
- **Radius**: Must be positive number (meters)
- **Address**: Optional but recommended

#### Chemical Parameters
- All measurements validated against scientific ranges
- Out-of-range values trigger warnings
- System prevents obviously invalid data (e.g., negative concentrations)

#### Dates
- Analysis dates cannot be in future
- Created/modified timestamps automatic
- Date formats localized

### Audit Trail

**Every change is logged**:
- Entity type (Water/Soil/User/Org)
- Entity ID
- Action (Create/Edit/Delete)
- Changed fields
- Old and new values
- User who made change
- Timestamp

**Access Audit Logs**:
- Available to Managers and Owners
- Path: `/audit/monitoring`
- Searchable and filterable
- Cannot be deleted or modified

### Attachments

**Supported**:
- Images: JPG, PNG, GIF, WebP
- Documents: PDF, DOC, DOCX, XLS, XLSX
- Maximum 10 files per deficiency
- Stored in Azure Blob Storage

**Upload Process**:
1. Select files from device
2. Upload to cloud storage
3. Link to deficiency record
4. Generate thumbnail (for images)
5. Track file metadata

**Viewing Attachments**:
- Click file to open/download
- Image preview in interface
- Document download

### Notifications

**Email Notifications Sent For**:
- Account creation (invitation)
- Email confirmation
- Password reset
- Role changes
- Assignment to deficiency
- Monitoring schedule reminders
- Critical danger level reports

**Notification Settings**:
- Configure in user profile
- Opt in/out of non-critical emails
- Cannot disable security emails

### Data Retention

**Active Data**:
- All deficiencies retained indefinitely
- Complete history maintained
- No automatic deletion

**Deleted Data**:
- Soft deletion (marked as deleted)
- Audit trail preserved
- Can be restored by Super Admin
- Hard deletion only by Super Admin

**User Data**:
- Account remains after organization removal
- Can join another organization
- Can request account deletion (GDPR compliance)

### Rate Limiting

**To prevent abuse**:
- API requests limited per IP address
- Default: 100 requests per minute
- Authentication increases limit
- Excessive requests return HTTP 429

**Limits**:
- Anonymous: 20 requests/minute
- Authenticated: 100 requests/minute
- Owner: 200 requests/minute

## Troubleshooting

### Common Issues

**Cannot Login**
- Verify email is confirmed
- Check password meets requirements
- Try password reset
- Check with organization Owner

**Cannot See Deficiency**
- May be marked private (organization members only)
- Verify you're logged in
- Check if deficiency was deleted

**Cannot Edit Deficiency**
- Check your role (minimum Supervisor)
- Must be creator or assigned user
- May be locked by monitoring scheme

**Report Not Generating**
- Verify role permissions
- Check date range (may be no data)
- Try different report format
- Check browser downloads folder

**Map Not Loading**
- Check internet connection
- Disable ad blockers
- Try different browser
- Clear browser cache

### Getting Help

**Technical Support**:
- Contact your organization Owner
- Use in-app feedback form
- Email: [support contact]

**Training Resources**:
- Video tutorials (available in app)
- PDF user manual
- Sample data templates
- Community forum

## Appendix

### Keyboard Shortcuts

- `Ctrl + K`: Open search
- `Ctrl + /`: Open help
- `Esc`: Close dialogs
- `Tab`: Navigate form fields

### API Documentation

Complete REST API documentation available at:
- Local: http://localhost:5000/swagger
- Production: [API URL]/swagger

### Compliance

NatureHelp follows:
- GDPR data protection standards
- Environmental reporting standards
- ISO 27001 security practices
- EU water quality directives
- Soil contamination guidelines

### Browser Compatibility

**Fully Supported**:
- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

**Limited Support**:
- Internet Explorer (not recommended)
- Older mobile browsers

### Updates & Changelog

Check for updates:
- Release notes in application
- GitHub releases
- Email announcements to Owners

---

**Document Version**: 1.0  
**Last Updated**: December 2025  
**Application Version**: Compatible with NatureHelp 1.x

