import { useContainerResource } from './container';
import { ContainerProvider } from '$/platform/di/ContainerContext';
import Routes from "./routes";

function Shell() {
  const container = useContainerResource();
  return (
    <ContainerProvider container={container}>
      <Routes />
    </ContainerProvider>
  );
}

export default Shell;
