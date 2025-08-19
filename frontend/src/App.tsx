import { useEffect, useState } from 'react';
import {
  Container, Stack, TextField, Button, Paper, Snackbar, Alert, IconButton, Tooltip
} from '@mui/material';
import { DataGrid, GridColDef, GridRenderCellParams } from '@mui/x-data-grid';
import DeleteIcon from '@mui/icons-material/Delete';
import { api, extractProblemMessage } from './api';
import { Contact, ContactCreateDto } from './types';

export default function App() {
  const [rows, setRows] = useState<Contact[]>([]);
  const [form, setForm] = useState<ContactCreateDto>({ name: '', email: '', phone: '', note: '' });
  const [error, setError] = useState<string | null>(null);
  const [busy, setBusy] = useState(false);

  const load = async () => {
    const res = await api.get<Contact[]>('/api/contacts');
    setRows(res.data);
  };

  useEffect(() => { load(); }, []);

  const create = async () => {
    try {
      setBusy(true);
      await api.post<Contact>('/api/contacts', form);
      setForm({ name: '', email: '', phone: '', note: '' });
      await load();
    } catch (e: any) {
      setError(extractProblemMessage(e));
    } finally {
      setBusy(false);
    }
  };

  const remove = async (id: number) => {
    try {
      setBusy(true);
      await api.delete(`/api/contacts/${id}`);
      await load();
    } catch (e: any) {
      setError(extractProblemMessage(e));
    } finally {
      setBusy(false);
    }
  };

  const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', width: 80 },
    { field: 'name', headerName: 'Name', flex: 1, minWidth: 180 },
    { field: 'email', headerName: 'Email', flex: 1.2, minWidth: 220 },
    { field: 'phone', headerName: 'Phone', flex: 1, minWidth: 160 },
    { field: 'note', headerName: 'Note', flex: 1.5, minWidth: 240 },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 120,
      sortable: false,
      filterable: false,
      renderCell: (params: GridRenderCellParams<Contact>) => (
        <Tooltip title="Delete">
          <span>
            <IconButton
              size="small"
              color="error"
              disabled={busy}
              onClick={() => remove(params.row.id)}
            >
              <DeleteIcon fontSize="small" />
            </IconButton>
          </span>
        </Tooltip>
      )
    }
  ];

  return (
    <Container sx={{ py: 4 }}>
      <Stack spacing={3}>
        <Paper sx={{ p: 2 }}>
          <Stack direction={{ xs: 'column', md: 'row' }} spacing={2}>
            <TextField
              label="Name" value={form.name}
              onChange={e => setForm({ ...form, name: e.target.value })} required
            />
            <TextField
              label="Email" type="email" value={form.email}
              onChange={e => setForm({ ...form, email: e.target.value })} required
            />
            <TextField
              label="Phone" value={form.phone}
              onChange={e => setForm({ ...form, phone: e.target.value })}
            />
            <TextField
              label="Note" value={form.note}
              onChange={e => setForm({ ...form, note: e.target.value })}
            />
            <Button variant="contained" onClick={create} disabled={busy}>
              Add
            </Button>
          </Stack>
        </Paper>

        <Paper sx={{ p: 1 }}>
          <div style={{ height: 520, width: '100%' }}>
            <DataGrid
              rows={rows}
              columns={columns}
              disableRowSelectionOnClick
              pageSizeOptions={[5, 10, 25]}
              initialState={{ pagination: { paginationModel: { pageSize: 10 } } }}
              loading={busy && rows.length === 0}
            />
          </div>
        </Paper>
      </Stack>

      <Snackbar open={!!error} autoHideDuration={4000} onClose={() => setError(null)}>
        <Alert severity="error" variant="filled" onClose={() => setError(null)}>
          {error}
        </Alert>
      </Snackbar>
    </Container>
  );
}
