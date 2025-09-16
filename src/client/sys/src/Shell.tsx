import { useContainerResource } from './container';
import { ContainerProvider } from '$/platform/di/ContainerContext';
import { Outlet } from 'react-router-dom';

function Shell() {
  const container = useContainerResource();
  return (
    <ContainerProvider container={container}>
      <Outlet />
    </ContainerProvider>
  );
}

export default Shell;
