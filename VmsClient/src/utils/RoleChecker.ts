export function checkRole(roles: string[], wantedRoles: string[]) {
    if(roles.includes('Administrator')) return true;

    for(let i = 0; i < roles.length; i++) {
        let role = roles[i];
        if(wantedRoles.includes(role)) return true;
    }

    return false;
}