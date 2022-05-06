// @material-ui/icons
import Dashboard from '@material-ui/icons/Dashboard';
import Person from '@material-ui/icons/Person';
import LibraryBooks from '@material-ui/icons/LibraryBooks';
// core components/views for Admin layout
import DashboardPage from './views/Dashboard/Dashboard';
import UserProfile from './views/UserProfile/UserProfile';
// core components/views for RTL layout
import Groups from './components/Groups/Groups';
import Agenda from './components/Agenda/Agenda';
import UserManager from './components/UserManager/UserManager'

// authentication component
import LoginView from './views/Login/LoginView';
import visualisations from './components/visualisations/visualisations';



const dashboardRoutes: {
  path: string,
  name: string,
  layout: string,
  component: any,
  roles: string[],
  icon?: any,}[] = [
  {
    path: '/login',
    name: 'Login Page',
    layout: '/agenda',
    component: LoginView,
    roles: []
  },
  {
    path: '/overview',
    name: 'Dashboard',
    icon: Dashboard,
    component: DashboardPage,
    layout: '/dashboard',
    roles: ["Weger", "Voedingsbeheerder", "Manager", "Administrator"]
  },
  {
    path: '/user',
    name: 'User Profile',
    icon: Person,
    component: UserProfile,
    layout: '/dashboard',
    roles: ["Administrator", "Werknemer"]
  },
  {
    path: '/groups',
    name: 'Table List',
    icon: 'content_paste',
    component: Groups,
    layout: '/dashboard',
    roles: ["Weger", "Voedingsbeheerder", "Manager", "Administrator"]
  },
  {
    path: '/visualisations',
    name: 'Visualisations',
    icon: 'content_paste',
    component: visualisations,
    layout: '/dashboard',
    roles: ["Administrator"]
  },
  {
    path: '/users',
    name: 'Typography',
    icon: LibraryBooks,
    component: UserManager,
    layout: '/dashboard',
    roles: ["Administrator"]
  },
  //agenda routes
  {
    path: '/overview',
    name: 'Typography',
    icon: LibraryBooks,
    component: Agenda,
    layout: '/agenda',
    roles: ["Administrator", "Agenda"]
  }
];

export default dashboardRoutes;