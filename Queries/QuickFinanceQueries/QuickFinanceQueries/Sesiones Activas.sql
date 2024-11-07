SELECT 
    spid AS SessionID,
    loginame AS LoginName,
    hostname AS HostName,
    program_name AS ProgramName,
    db_name(dbid) AS DatabaseName,
    cmd AS Command,
    status AS Status,
    login_time AS LoginTime,
    last_batch AS LastActivityTime
FROM 
    sys.sysprocesses
WHERE 
    db_name(dbid) in  ('QuickFinanceDB','QuickFinanceDB-Copy' );
